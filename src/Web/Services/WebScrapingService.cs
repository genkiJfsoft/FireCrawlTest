namespace Web.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Firecrawl;


public class WebScrapingService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl = "https://api.firecrawl.dev ";
    private readonly string _apiKey = "fc-05f208106262429daa83520660841dda"; // Replace with your actual API key
    private readonly IFirecrawlService firecrawl;

    public WebScrapingService(HttpClient httpClient, IConfiguration configuration, IFirecrawlService firecrawl)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        //_apiUrl = configuration["DeepSeek:ApiUrl"] ?? throw new ArgumentNullException("DeepSeek:ApiUrl");
        //_apiKey = configuration["DeepSeek:ApiKey"] ?? throw new ArgumentNullException("DeepSeek:ApiKey");

        this.firecrawl = firecrawl;
    }

    public async Task<List<string>> ScrapeAndSearchAsync(string url, List<string> keywords)
    {
        // Create the payload
        var requestPayload = new
        {
            url = url,
            keywords = keywords
        };

        // Configure the request
        var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl)
        {
            Content = JsonContent.Create(requestPayload)
        };

        // Add Authorization header
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        // Send the request
        var response = await _httpClient.SendAsync(request);

        // Ensure success and parse response
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<DeepSeekResponse>();

        return result?.Results ?? new List<string>();
    }

    // Model to parse DeepSeek response
    private class DeepSeekResponse
    {
        public List<string>? Results { get; set; }
    }

}


