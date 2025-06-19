using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using RBACDemo.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourdomain.com",
            ValidAudience = "yourdomain.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-32-character-secret-key12353425424334122313123123"))
        };
    });

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


// Seed users and roles on startup
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // LMS Roles and Users
    var lmsRoles = new[] { "Admin", "Instructor", "Student", "Guest" };
    var lmsUsers = new[]
    {
        new { Username = "lmsadmin", Password = "Admin@123", Role = "Admin" },
        new { Username = "instructor1", Password = "Instr@123", Role = "Instructor" },
        new { Username = "student1", Password = "Stud@123", Role = "Student" },
        new { Username = "guest1", Password = "Guest@123", Role = "Guest" }
    };

    // Retail Bank Roles and Users
    var bankRoles = new[] { "Admin", "Teller", "Auditor", "Customer" };
    var bankUsers = new[]
    {
        new { Username = "bankadmin", Password = "Admin@123", Role = "Admin" },
        new { Username = "teller1", Password = "Tell@123", Role = "Teller" },
        new { Username = "auditor1", Password = "Audi@123", Role = "Auditor" },
        new { Username = "customer1", Password = "Cust@123", Role = "Customer" }
    };

    // Ensure roles exist
    foreach (var role in lmsRoles.Concat(bankRoles))
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Create and assign users
    foreach (var user in lmsUsers.Concat(bankUsers))
    {
        var identityUser = new IdentityUser { UserName = user.Username, Email = $"{user.Username}@example.com" };
        var existingUser = await userManager.FindByNameAsync(user.Username);
        if (existingUser == null)
        {
            var result = await userManager.CreateAsync(identityUser, user.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(identityUser, user.Role);
            }
        }
    }
}

app.Run();