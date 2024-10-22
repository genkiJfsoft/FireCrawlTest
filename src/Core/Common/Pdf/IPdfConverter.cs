namespace Core.Common.Pdf;

internal interface IPdfConverter
{
    Task<byte[]> FromUrlAsync(string url, string? headerText = null, bool displayPageNumber = false);

    Task<byte[]> FromHtmlAsync(string html, string? headerText = null, bool displayPageNumber = false);
}
