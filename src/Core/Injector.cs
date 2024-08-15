using System.Reflection;
using Core.Common.Data;
using Core.Common.Exceptions;
using Core.Common.Mailer;
using Core.Common.Security;
using Core.Identities.Data;
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
        services.AddPersistenceProvider(configuration, isDevelopment);
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

    private static void AddPersistenceProvider(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        string? connectionString = configuration.GetConnectionString(DataContext.ConnectionStringName);
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        services.AddScoped<ISaveChangesInterceptor, TimestampableDataInterceptor>();
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
    }

    private static void AddIdentityAuth(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddIdentityCookies();

        services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddSignInManager()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        services.AddAuthorizationCore(options =>
        {
            // Register Policies
            options.AddPolicy(KnownPolicies.CanPurge, policy => policy.RequireRole(KnownRoles.Administrator));
        });

    }
}
