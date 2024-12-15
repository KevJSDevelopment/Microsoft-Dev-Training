// See https://aka.ms/new-console-template for more information

// await new SwaggerClientGenerator().GenerateClient();

using BlogApi;

var httpClient = new HttpClient();
var apiBaseUrl = "http://localhost:5020";

var client = new BlogApiClient(apiBaseUrl, httpClient);

var blogs = await client.BlogsAllAsync();

foreach(var blog1 in blogs)
{
    Console.WriteLine($"{blog1.Title}: {blog1.Body}");
}

await client.BlogsDELETEAsync(0);

var blog = new Blog
{
    Title = "My title",
    Body = "My body"
};

await client.BlogsPOSTAsync(blog);

/*Console.WriteLine("Hello, World!");


var httpClient = new HttpClient();
var apiBaseUrl = "http://localhost:5230";

var httpResults = await httpClient.GetAsync($"{apiBaseUrl}/blogs");

if(httpResults.StatusCode != System.Net.HttpStatusCode.OK)
{
    Console.WriteLine("Failed to fetch blogs.");
    return;
};

var blogStream = await httpResults.Content.ReadAsStreamAsync();

var options = new System.Text.Json.JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

var blogs = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Blog>>(blogStream, options);

if(blogs != null)
{
    foreach( var blog in blogs)
    {
        Console.WriteLine($"{blog.Title}: {blog.Body}");
    }
};

class Blog 
{
    public required string Title { get; set; }
    public required string Body { get; set; }  
};
*/