using Serilog;
using Serilog.Events;

// настройка Serilog ДО создания builder,
Log.Logger = new LoggerConfiguration()
    // минимальный уровень логирования Debug (самый подробный)
    .MinimumLevel.Debug()

    // для системных компонентов ASP.NET Core только Warning
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)

    
    // {Timestamp} время, {Level:u3} уровень (3 символа: DBG/INF/WRN/ERR) {Message} текст сообщения, {NewLine}{Exception} исключение если есть
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    
    // запись в файл
    // rollingInterval: каждый день создаётся новый файл (logs/log-20260101.txt) retainedFileCountLimit: хранить не более 7 файлов
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// замена встроенного логирования ASP.NET Core на Serilog
builder.Host.UseSerilog();

var app = builder.Build();

// записывает метод, путь и статус-код в лог автоматически
app.UseSerilogRequestLogging();

// GET /log/demo демонстрирует все уровни логирования
app.MapGet("/log/demo", (ILogger<Program> logger) =>
{
    // Trace самый подробный уровень
    logger.LogTrace("Trace: очень детальная информация о выполнении");

    // Debug отладочная информация
    logger.LogDebug("Debug: запрос поступил на эндпоинт /log/demo");

    // Information стандартные рабочие события
    logger.LogInformation("Information: успешно обработан запрос /log/demo");

    // Warning что-то нестандартное, но не ошибка
    logger.LogWarning("Warning: параметр не передан, использовано значение по умолчанию");

    // Error ошибка, которую приложение смогло обработать
    logger.LogError("Error: произошла обработанная ошибка");

    // Critical критическая ошибка, требующая немедленного внимания
    logger.LogCritical("Critical: критическая ситуация в системе");

    return Results.Ok(new { message = "Все уровни логирования записаны. Проверь консоль и папку logs/" });
});

// GET /log/info логирование с параметрами (structured logging)
app.MapGet("/log/info", (ILogger<Program> logger) =>
{
    var userId = 42;
    var action = "просмотр списка";

    logger.LogInformation("Пользователь {UserId} выполнил действие: {Action}", userId, action);

    return Results.Ok(new { userId, action });
});

// GET /log/warning логирование предупреждения с контекстом
app.MapGet("/log/warning", (ILogger<Program> logger) =>
{
    var itemsRequested = 1000;
    var maxAllowed = 100;

    logger.LogWarning(
        "Запрошено {ItemsRequested} элементов, превышает максимум {MaxAllowed}. Возвращено {MaxAllowed}",
        itemsRequested, maxAllowed, maxAllowed);

    return Results.Ok(new { warning = "Превышен лимит", returned = maxAllowed });
});

// GET /log/error логирование исключения
app.MapGet("/log/error", (ILogger<Program> logger) =>
{
    try
    {
        // намеренный вызов исключения 
        throw new InvalidOperationException("Тестовое исключение для демонстрации логирования");
    }
    catch (Exception ex)
    {
        // передача исключения первым параметром 
        logger.LogError(ex, "Перехвачено исключение в эндпоинте /log/error");
        return Results.Problem("Произошла ошибка, она записана в лог");
    }
});

// GET /ping эндпоинт, его запрос будет залогирован через UseSerilogRequestLogging
app.MapGet("/ping", (ILogger<Program> logger) =>
{
    logger.LogInformation("Эндпоинт /ping вызван успешно");
    return Results.Ok("pong");
});

// гарантия записи всех буферизованных логов перед завершением приложения
app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();