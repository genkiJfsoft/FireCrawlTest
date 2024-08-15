using Microsoft.Extensions.Configuration;

namespace Core.Common.Configurations;

internal static class ConfigurationExtensions
{
    /// <summary>
    /// Attempts to bind a new instance of <typeparamref name="TConfiguration"/> to the configuration section specified
    /// by the key by matching property names against configuration keys recursively.
    /// </summary>
    /// <typeparam name="TConfiguration"></typeparam>
    /// <param name="configuration"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static TConfiguration GetSection<TConfiguration>(
      this IConfiguration configuration,
      string key
    ) where TConfiguration : class
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        var instance = Activator.CreateInstance<TConfiguration>();
        configuration.Bind(key, instance);
        return instance;
    }
}
