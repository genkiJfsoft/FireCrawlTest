namespace Web.Services;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;


public class JsonPlaceholderService
{
    private readonly HttpClient _httpClient;

    public JsonPlaceholderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/photos");

    }


    // Fetch all posts
    public async Task<List<Post>> GetPostsAsync()
    {
        var posts = await _httpClient.GetFromJsonAsync<List<Post>>("posts");
        return posts ?? new List<Post>();
    }
    // Fetch a single post by ID
    public async Task<Post?> GetPostByIdAsync(int id)
    {
        var post = await _httpClient.GetFromJsonAsync<Post>($"posts/{id}");
        return post;
    }

    public async Task<List<Photo>> GetPhotosAsync()
    {
        var photos = await _httpClient.GetFromJsonAsync<List<Photo>>("photos");
        return photos ?? new List<Photo>();

    }

    // Fetch a single photo by ID
    public async Task<Photo?> GetPhotoByIdAsync(int id)
    {
        var photo = await _httpClient.GetFromJsonAsync<Photo>($"photos/{id}");
        return photo;
    }
}
public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
}

public class Photo
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? ThumbnailUrl { get; set; }
}
