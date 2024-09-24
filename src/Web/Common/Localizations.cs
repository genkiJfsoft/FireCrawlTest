using System.Globalization;

namespace Web.Common;

/// <summary>
/// Localization options constants
/// </summary>
internal sealed class Localizations
{
    public const string ResourcesPath = "Localization";

    public static List<CultureInfo> SupportedCultures => [new("en"), new("ms")];

    public static CultureInfo DefaultCulture => SupportedCultures[0];
}
