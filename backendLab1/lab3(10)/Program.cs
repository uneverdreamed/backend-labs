using System.Text;

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

app.Run();

