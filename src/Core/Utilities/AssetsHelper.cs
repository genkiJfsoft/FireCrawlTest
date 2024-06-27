using System.Reflection;

namespace Core.Utilities;

internal class AssetsHelper
{
    public static string ReadAsString(string resourceName)
    {
        resourceName = resourceName.Replace("/", ".");
        var assembly = Assembly.GetExecutingAssembly();
        var fullResourceName = $"{assembly.GetName().Name}.{resourceName}";
        using var stream = assembly.GetManifestResourceStream(fullResourceName) ?? throw new InvalidOperationException("Could not load manifest resource stream.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
