using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Core.Common.Pdf;

internal class PdfConverter(ILogger<PdfConverter> logger) : IPdfConverter
{
    public async Task<byte[]> FromUrlAsync(string url, string? headerText = null, bool displayPageNumber = false)
    {
        try
        {
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(
                new LaunchOptions
                {
                    Headless = true,
                    Args = [
                        "--no-sandbox"
                    ]
                });

            await using var page = await browser.NewPageAsync();
            await page.GoToAsync(url);

            var output = await page.PdfDataAsync(OutputOptions(headerText, displayPageNumber));

            return output;
        }
        catch (Exception ex)
        {
            logger.LogError("Error converting PDF from URL: {ex}", ex);
            throw;
        }
    }

    public async Task<byte[]> FromHtmlAsync(string html, string? headerText = null, bool displayPageNumber = false)
    {
        try
        {
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(
                new LaunchOptions
                {
                    Headless = true,
                    Args = [
                        "--no-sandbox"
                    ]
                });

            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);

            var output = await page.PdfDataAsync(OutputOptions(headerText, displayPageNumber));

            return output;
        }
        catch (Exception ex)
        {
            logger.LogError("Error converting PDF from HTML: {ex}", ex);
            throw;
        }
    }

    private static PdfOptions OutputOptions(string? headerText = null, bool displayPageNumber = false)
    {
        string headerTemplate = $@"
        <div id=""header-template"" style=""font-family:'Sarabun'; font-size:11px; width: 100%; padding-right: 55px; padding-left: 55px; margin-right: auto; margin-left: auto; margin-top: 10px;"">
           {headerText ?? ""}
        </div>";

        string footerText = displayPageNumber ? @"<span class=""pageNumber""></span> of <span class=""totalPages""></span>" : string.Empty;
        string footerTemplate = $@"
        <div id=""footer-template"" style=""font-family:'Sarabun'; text-align: right; font-size:11px; width: 100%; padding-right: 55px; padding-left: 55px; margin-right: auto; margin-left: auto; margin-bottom: 10px;"">
            {footerText}
        </div>";

        return new()
        {
            Format = PaperFormat.A4,
            DisplayHeaderFooter = true,
            MarginOptions = new MarginOptions { Top = "80px", Right = "20px", Bottom = "80px", Left = "20px" },
            PreferCSSPageSize = true,
            HeaderTemplate = headerTemplate,
            FooterTemplate = footerTemplate,
            PrintBackground = true
        };
    }
}
