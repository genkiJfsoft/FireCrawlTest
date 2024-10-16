using System.Reflection;
using Core.Common.Data;
using Core.Common.Exceptions;
using Core.Common.Mailer;
using Core.Common.Security;
using Core.Identity.Data;
using Core.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        services.AddDataProvider(configuration, isDevelopment);
        services.AddScoped<IMailer, Mailer>();

        services.AddIdentityAuth();

        // Register Features
        var assemblyToScan = Assembly.GetExecutingAssembly();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assemblyToScan);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionPipelineBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationPipelineBehavior<,>));
        });

        return services;
    }

    private static void AddDataProvider(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        string? connectionString = configuration.GetConnectionString(DataContext.ConnectionStringName);
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        services.AddScoped<ISaveChangesInterceptor, TimestampableDataInterceptor>();

        services.AddDbContext<DataContext>((sp, options) => {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);

            if (isDevelopment)
            {
                options.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        });

        services.AddScoped<IDbContextFactory<DataContext>, DataContextFactory>();
        services.AddScoped<IDataContext>(sp => sp.GetRequiredService<IDbContextFactory<DataContext>>().CreateDbContext());

        services.AddScoped<DataContextInitializer>();
    }

    private static void AddIdentityAuth(this IServiceCollection services)
    {
        services.AddDefaultIdentity<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>();

        services.AddAuthorization(options =>
            options.AddPolicy(KnownPolicies.CanPurge, policy => policy.RequireRole(KnownRoles.Administrator)));

        services.AddScoped<IEmailSender<User>, IdentityMailer>();
    }
}
