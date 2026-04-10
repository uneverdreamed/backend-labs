using laba9.settings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection(AppSettings.SectionName));

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(DatabaseSettings.SectionName));

var app = builder.Build();

// GET /config возвращает текущую конфигурацию приложения
app.MapGet("/config", (IOptions<AppSettings> appSettings, IOptions<DatabaseSettings> dbSettings) =>
{
    return Results.Ok(new
    {
        environment = app.Environment.EnvironmentName,
        appSettings = appSettings.Value,
        databaseSettings = dbSettings.Value
    });
});

// GET /config/app только AppSettings
app.MapGet("/config/app", (IOptions<AppSettings> settings) =>
{
    return Results.Ok(settings.Value);
});

// GET /config/db только DatabaseSettings
app.MapGet("/config/db", (IOptions<DatabaseSettings> settings) =>
{
    return Results.Ok(settings.Value);
});

// GET /config/raw чтение конфигурации напрямую через IConfiguration (без типизации)
app.MapGet("/config/raw", (IConfiguration config) =>
{
    return Results.Ok(new
    {
        appName = config["AppSettings:ApplicationName"],
        version = config["AppSettings:Version"],
        maxItems = config["AppSettings:MaxItemsPerPage"],
        connectionString = config["DatabaseSettings:ConnectionString"],
        aspnetEnv = config["ASPNETCORE_ENVIRONMENT"]
    });
});

// GET /config/env демонстрирует текущую среду и поведение приложения
app.MapGet("/config/env", (IWebHostEnvironment env, IOptions<AppSettings> settings) =>
{
    return Results.Ok(new
    {
        environmentName = env.EnvironmentName,
        isDevelopment = env.IsDevelopment(),
        isStaging = env.IsStaging(),
        isProduction = env.IsProduction(),
        loggingEnabled = settings.Value.EnableLogging,
        maxItemsPerPage = settings.Value.MaxItemsPerPage
    });
});

app.Run();