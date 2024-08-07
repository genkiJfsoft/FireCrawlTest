using System.Reflection;
using Core.Providers.Data;
using Core.Providers.Data.Interceptors;
using Core.Providers.Mailer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core;

public static class Injector
{
    /// <summary>
    /// Register core dependencies
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="isDevelopment"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationCore(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        // Register Providers
        services.AddSingleton(TimeProvider.System);
        services.AddDataContext(configuration, isDevelopment);
        services.AddScoped<IMailer, Mailer>();

        // Register Features
        var assemblyToScan = Assembly.GetExecutingAssembly();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assemblyToScan));

        return services;
    }

    private static IServiceCollection AddDataContext(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        string? connectionString = configuration.GetConnectionString(DataContext.ConnectionStringName);
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        services.AddScoped<ISaveChangesInterceptor, EntityTimestampableInterceptor>();
        services.AddDbContext<DataContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
            if (isDevelopment)
            {
                options.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        });
        return services;
    }
}
