using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
    options.SerializerOptions.WriteIndented = true; 
});

var app = builder.Build();

app.Map("/", () =>
{
    var response = "Good job <br> <img src='https://i.pinimg.com/736x/3a/38/a0/3a38a0cce3cfc3857a3543fb3795b1fb.jpg'>";
    return Results.Content(response, "text/html");

});

app.MapGet("/api/info", () =>
{
    return Results.Ok(new
    {
        appName = "backendLab1",
        author = "Федукина Мария",
        description = "это первая лаба по бэкенд-разработке"
    });
});

app.MapGet("/api/time", () =>
{
    return Results.Ok(new
    {
        currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        timezone = TimeZoneInfo.Local.DisplayName
    });
});

app.Run();

