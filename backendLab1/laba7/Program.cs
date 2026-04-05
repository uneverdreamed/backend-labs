using laba7.middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseMiddleware<BlockPathMiddleware>();
app.UseMiddleware<RequestTraceMiddleware>();
app.UseMiddleware<EndpointTimingMiddleware>();

// GET /ping (простой эндпоинт для проверки работы конвейера)
app.MapGet("/ping", () => "pong");
 
// GET /trace (возвращает TraceId, сохранённый в контексте middleware RequestTrace)
app.MapGet("/trace", (HttpContext context) =>
{
    var traceId = context.Items["TraceId"]?.ToString() ?? "TraceId не найден";
    return Results.Ok(new { traceId });
});
 
// GET /error (намеренно выбрасывает исключение для демонстрации обработки ошибок)
app.MapGet("/error", () =>
{
    throw new InvalidOperationException("Тестовое исключение для проверки обработки ошибок");
});
 
// GET /blocked/test (для проверки BlockPathMiddleware (403))
app.MapGet("/blocked/test", () => "Этот ответ никогда не будет получен");
 
app.Run();
