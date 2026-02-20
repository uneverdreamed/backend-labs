using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// html 
app.MapGet("/api/html", () =>
{
    var html = """
        <!DOCTYPE html>
        <html lang="ru">
        <head><meta charset="UTF-8"><title>HTML-ответ</title></head>
        <body>
            <h1>пупупу</h1>
            <p>это пример HTML-ответа от сервера</p>
        </body>
        </html>
        """;
    return Results.Content(html, "text/html");
});

// text
app.MapGet("/api/text", () =>
{
    return Results.Content("это текстовый ответ от сервера. пупупу", "text/plain; charset=utf-8");
});


// json
app.MapGet("api/json", () =>
{
    var data = new
    {
        message = "это JSON-ответ от сервера",
        value = 1234567890,
        items = new[] { "пупу", "пупу", "пупу" }
    };
    return Results.Json(data);
});

app.Run();

