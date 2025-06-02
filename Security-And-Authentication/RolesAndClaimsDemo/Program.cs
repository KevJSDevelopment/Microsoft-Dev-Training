using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>(options => options.UseInMemoryDatabase("AuthDemoDB"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        if(context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == StatusCodes.Status200OK)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }
        
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))
    .AddPolicy("ItDepartment", policy => policy.RequireClaim("Department", "IT"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/api/admin-only", () => "Admin Only Area")
    .RequireAuthorization("AdminOnly");

app.MapGet("/api/user-claim-check", () => "Access granted to IT department users only")
    .RequireAuthorization("ItDepartment");

app.MapGet("/account/login", () => "User route for login");

var roles = new[] { "Admin", "User" };

app.MapPost("/api/create-role", async (RoleManager<IdentityRole> roleManager) =>
{
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    return Results.Ok("Roles created");
});

app.MapPost("/api/assign-role", async (UserManager<IdentityUser> userManager) =>
{
    var user = new IdentityUser { UserName = "testuser", Email = "testuser@example.com" };
    await userManager.CreateAsync(user, "Password123!");
    await userManager.AddToRoleAsync(user, "Admin");

    var isInRole = await userManager.IsInRoleAsync(user, "Admin");

    return isInRole ? Results.Ok("User assigned to Admin role") : Results.BadRequest("Failed to assign role");
});

app.MapPost("/api/login", async (SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) =>
{
    var user = await userManager.FindByEmailAsync("testuser@example.com");
    if (user == null) return Results.NotFound("User not found");

    await signInManager.SignInAsync(user, isPersistent: false);
    return Results.Ok("User signed in!");
});

app.MapPost("/api/add-claim", async (UserManager<IdentityUser> userManager) =>
{
    var user = await userManager.FindByEmailAsync("testuser@example.com");
    if (user == null) return Results.NotFound("User not found");

    await userManager.AddClaimAsync(user, new Claim("Department", "IT"));
    return Results.Ok("User added to IT department!");
});

app.Run();

