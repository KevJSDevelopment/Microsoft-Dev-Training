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
builder.Services.AddAuthorizationBuilder();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/api/admin-only", () => "Admin Only Area")
    .RequireAuthorization();

app.MapGet("/api/user-claim-check", () => "Access granted to IT department users only")
    .RequireAuthorization(policy => policy.RequireClaim("Department", "IT"));

app.MapGet("/account/login", () => "User route for login");

var roles = new[] { "Admin", "User" };

app.Run();

