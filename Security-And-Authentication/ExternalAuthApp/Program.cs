using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
var app = builder.Build();

var clientSecrets = new ClientSecrets
{
    ClientId = builder.Configuration["Authentication:Google:ClientId"],
    ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
};

var scopes = new[] { "https://www.googleapis.com/auth/userinfo.profile" };

var datastore = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GoogleAuthStore");

var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
    clientSecrets,
    scopes,
    "user",
    CancellationToken.None,
    new FileDataStore(datastore, true)
);

app.MapGet("/login", async (context) => await context.Response.WriteAsync("Login endpoint. Use Google OAuth to authenticate."));

app.Run();
