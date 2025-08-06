using Core.Extensions;
using Loggers.Contracts;
using Loggers.Extensions;
using Loggers.Publishers;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// We need to initlialize the EventReaderSingleton instance to start reading events from the event source.
_ = EventReaderSingleton.Instance;

// Add the event source to the DI container
builder.Services.AddSingleton<IPublisher>(EventSourcePublisher.Log);

// Initialize the logging services
builder.Services.InitializeServices(builder.Configuration);
builder.Services.InitializeLogging(builder.Configuration);
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// In Program.cs or your controller
app.MapPost("/api/generate-exception", (ILogger<Program> logger) =>
{
    var generator = new ExceptionGenerator();
    generator.GenerateAndLogRandomException(ex => logger.LogError(ex, "Random exception generated"));
    return Results.Ok();
});

app.Run();
