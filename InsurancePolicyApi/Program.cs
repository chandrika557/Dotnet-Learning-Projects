var builder = WebApplication.CreateBuilder(args); // Create a WebApplication builder

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/hello", () => TypedResults.Ok("Hello from the Insurance Policy API!"))
    .WithName("GetHello")
    .WithSummary("Get a greeting")
    .WithDescription("Returns a simple greeting to confirm the API is reachable.")
    .Produces<string>(StatusCodes.Status200OK);

app.MapGet("/api/hello/{name}", (string name) =>
        TypedResults.Ok($"Hello, {name}!"))
    .WithName("GetPersonalizedHello")
    .WithSummary("Get a personalized greeting")
    .WithDescription("Returns a greeting that includes the name from the URL.")
    .Produces<string>(StatusCodes.Status200OK);

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
