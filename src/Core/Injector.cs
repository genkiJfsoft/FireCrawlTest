using System.Reflection;
using Core.Providers.Mailer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddScoped<IMailer, Mailer>();

        // Register Features
        var assemblyToScan = Assembly.GetExecutingAssembly();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assemblyToScan));

        return services;
    }
}
