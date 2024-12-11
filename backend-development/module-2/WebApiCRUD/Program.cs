var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// app.UseHttpLogging();

/* 
    Middleware handles logic in order it is called. Meaning all logic after next.Invoke will occur after the next.Invoke is fully completed.
    
    Logic before next.invoke completes first, then all other logic follows, and logic after next.invoke is last
    Example, the below code will execute in this order:
        Logic Before 1
        Logic Before 2
        Logic Before 3
        Logic After 3
        Logic After 2
        Logic After 1
*/

app.Use(async (context, next) => {
    Console.WriteLine("Logic Before 1");
    await next.Invoke();
    Console.WriteLine("Logic After 1");
});
app.Use(async (context, next) => {
    Console.WriteLine("Logic Before 2 ");
    await next.Invoke();
    Console.WriteLine("Logic After 2");
});
app.Use(async (context, next) => {
    Console.WriteLine("Logic Before 3");
    await next.Invoke();
    Console.WriteLine("Logic After 3");
});

app.MapGet("/", () => "Hello World!");
app.MapGet("/index", () => "index page");

app.Run();
