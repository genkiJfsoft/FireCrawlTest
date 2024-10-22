using System.Reflection;
using Scriban;

namespace Core.Common.Assets;

public static class AssetsHelper
{
    /// <summary>
    /// Asynchronously reads all the text in an embedded file.
    /// <para />This loads the specified embedded file from the executing assembly, or, <paramref name="fromAssembly"/>, if specified.
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="fromAssembly"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static Task<string> ReadEmbeddedTextFileAsync(string resourceName, Assembly? fromAssembly = null)
    {
        resourceName = resourceName.Replace("/", ".");
        var assembly = fromAssembly ?? Assembly.GetExecutingAssembly();
        var fullResourceName = $"{assembly.GetName().Name}.{resourceName}";
        using var stream = assembly.GetManifestResourceStream(fullResourceName) ?? throw new InvalidOperationException("Could not load manifest resource stream.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEndAsync();
    }

    /// <summary>
    /// Asynchronously reads and parses the text in an embedded file into a <see cref="Template"/>
    /// <para />This loads the specified embedded file from the executing assembly, or, <paramref name="fromAssembly"/>, if specified.
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="fromAssembly"></param>
    public static async Task<Template> ReadEmbeddedTemplateFileAsync(string resourceName, Assembly? fromAssembly = null)
    {
        var content = await ReadEmbeddedTextFileAsync(resourceName, fromAssembly);
        return Template.Parse(content);
    }
}
