using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Logging services
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<JwtAuthenticationMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.Use(async (context, next) => {
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Global exception caught: {ex}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected error occurred. Please try again later");
    }
});

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

List<User> users =
[ 
    new User{ Name = "Alice", Email = "alice@example.com", Password = "testP@ass123"}, 
    new User{ Name = "JohnDoe", Email = "JohnD@example.com", Password = "testP@ss123!"}
];

// For better performance with larger lists:
app.MapGet("/users", Results<Ok<IEnumerable<UserDto>>, ProblemHttpResult>() => {
    try
    {
        return TypedResults.Ok(users.Select(u => new UserDto {
            Name = u.Name,
            Email = u.Email
        }));
    }
    catch (Exception ex)
    {
        return TypedResults.Problem("An error occurred while retrieving users");
    }
});

app.MapGet("/users/{id}", Results<Ok<User>, NotFound>(int id) => {
    if(id < 0 || id >= users.Count)
    {
        return TypedResults.NotFound();
    } else return TypedResults.Ok(users[id]);
});

app.MapPost("/users", Results<ValidationProblem, Created<User>> (User user) => {
    try
    {
        // Validate user
        if (!user.Validate())
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Error", new[] { "Invalid user data" } }
            });

        // Check for duplicate email
        if (users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Error", new[] { "Email already exists" } }
            });

        user.Password = HashPassword(user.Password);
        users.Add(user);
        return TypedResults.Created($"/users/{users.Count - 1}", user);
    }
    catch (Exception ex)
    {
        return TypedResults.ValidationProblem(new Dictionary<string, string[]>
        {
            { "Error", new[] { "An error occurred while processing your request" } }
        });
    }
});

app.MapPut("/users/{id}", Results<NotFound, Ok<User>, ValidationProblem> (int id, User user) => {
    try {
        if(id < 0 || users.Count - 1 < id) return TypedResults.NotFound();
        
        if (!user.Validate())
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Error", new[] { "Invalid user data" } }
            });

        User currentUser = users[id];
        currentUser.Name = user.Name;
        currentUser.Email = user.Email;
        currentUser.Password = HashPassword(user.Password);
        return TypedResults.Ok(currentUser);
    }
    catch (Exception ex)
    {
        return TypedResults.ValidationProblem(new Dictionary<string, string[]>
        {
            { "Error", new[] { "An error occurred while processing your request" } }
        });
    }
});

app.MapDelete("/users/{id}", Results<NotFound, NoContent, ProblemHttpResult>(int id) => {
    try {
        if(id < 0 || users.Count - 1 < id) return TypedResults.NotFound();
        users.RemoveAt(id);
        return TypedResults.NoContent();
    }
    catch (Exception ex) {
        return TypedResults.Problem("An error occurred while deleting the user");
    }
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

string HashPassword(string password)
{
    using var sha256 = SHA256.Create();
    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    return Convert.ToBase64String(hashedBytes);
}
