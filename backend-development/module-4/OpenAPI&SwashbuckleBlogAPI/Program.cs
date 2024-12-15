using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var blogs = new List<Blog>
{
    new Blog { Title = "first blog post", Body = "This is my first blog post"},
    new Blog { Title = "second blog post", Body = "This is my second blog post"},
};

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "I am root!");

app.MapGet("/blogs", () => {
    return blogs;
});

app.MapGet("/blogs/{id}", Results<Ok<Blog>, NotFound>(int id) => {
    if(id < 0 || id >= blogs.Count)
    {
        return TypedResults.NotFound();
    } else return TypedResults.Ok(blogs[id]);
});

app.MapPost("/blogs", (Blog blog) => {
    blogs.Add(blog);
    return Results.Created($"/blogs/{blogs.Count - 1}", blog);
});

app.MapDelete("/blogs", Results<NoContent, NotFound>(int id) => {
    if(id < 0 || id >= blogs.Count) return TypedResults.NotFound(); 
    else {
        blogs.RemoveAt(id);
        return TypedResults.NoContent();
    }
});

app.Run();


class Blog 
{
    public required string Title { get; set; }
    public required string Body { get; set; }  
};