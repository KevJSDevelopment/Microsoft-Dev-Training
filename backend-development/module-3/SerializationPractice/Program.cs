using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.AspNetCore.OpenApi;
using Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var samplePerson = new Person {UserName = "Kevin", UserAge = 28 };


if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/manual-json", () => {
    var jsonString = JsonSerializer.Serialize(samplePerson);
    return TypedResults.Text(jsonString, "application/json");
});

app.MapGet("/custom-json", () => {
    var options = new JsonSerializerOptions {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
    
    var jsonString = JsonSerializer.Serialize(samplePerson, options);
    return TypedResults.Text(jsonString, "application/json");
});

app.MapGet("/json", () => {
    return TypedResults.Json(samplePerson);
});

//.NET serializes to json by default, you only use the above options if customization is needed.
app.MapGet("/auto", () => {
    return samplePerson;
}).WithOpenApi(operation => {
    //operation.Parameters[0].Description = "The sample person json object";
    operation.Summary = "Get a single person";
    operation.Description = "Returns a single blog";
    return operation;
}).Produces<Person>(
    StatusCodes.Status200OK
).Produces(StatusCodes.Status404NotFound);

//xml serializer
app.MapGet("/xml", () => {
    var xmlSerializer = new XmlSerializer(typeof(Person));
    var stringWriter = new StringWriter();
    xmlSerializer.Serialize(stringWriter, samplePerson);
    var xmlOutput = stringWriter.ToString();

    return TypedResults.Text(xmlOutput, "application/xml");
});

app.Run();

public class Person {
    required public string UserName { get; set; }
    required public int UserAge { get; set; }
}