using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

app.MapGet("/users", () => users);

app.MapGet("/users/{id}", Results<Ok<User>, NotFound>(int id) => {
    if(id < 0 || id >= users.Count)
    {
        return TypedResults.NotFound();
    } else return TypedResults.Ok(users[id]);
});

app.MapPost("/users", (User user) => {
    users.Add(user);
    return TypedResults.Created($"/users/{users.Count - 1}", user);
});

app.MapPut("/users/{id}", Results<NotFound, Ok<User>> (int id, User user) => {
    if(id < 0 || users.Count - 1 < id) return TypedResults.NotFound();
    User currentUser = users[id];
    currentUser.Name = user.Name;
    currentUser.Email = user.Email;
    currentUser.Password = user.Password;
    return TypedResults.Ok(currentUser);
});

app.MapDelete("/users/{id}", Results<NotFound, NoContent>(int id) => {
    if(id < 0 || users.Count - 1 < id) return TypedResults.NotFound();
    users.RemoveAt(id);
    return TypedResults.NoContent();
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
