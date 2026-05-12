using System.Net;
using System.Text.Json;

namespace laba12.middleware
{
    // middleware для глобальной обработки необработанных исключений
    // перехватывает любые ошибки, которые не были пойманы фильтрами или контроллерами,
    // и возвращает клиенту единообразный JSON-ответ вместо стектрейса или пустого 500
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // передаём запрос следующему компоненту конвейера
                await _next(context);
            }
            catch (Exception ex)
            {
                // сюда попадают только те исключения, которые не обработал ни один фильтр
                _logger.LogError(ex, "Необработанное исключение: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // определяется HTTP-код и сообщение в зависимости от типа исключения
            var (statusCode, message) = exception switch
            {
                // бизнес-логика выбросила ArgumentException — неверные данные от клиента
                ArgumentException argEx => (StatusCodes.Status400BadRequest, argEx.Message),

                // попытка обратиться к ресурсу, которого нет
                KeyNotFoundException knfEx => (StatusCodes.Status404NotFound, knfEx.Message),

                // операция недоступна (например, удаление заблокированного ресурса)
                InvalidOperationException ioEx => (StatusCodes.Status422UnprocessableEntity, ioEx.Message),

                // все остальные исключения — внутренняя ошибка сервера
                _ => (StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера")
            };

            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = statusCode;

            // формирование JSON-ответ — клиент всегда получает предсказуемую структуру
            var response = new
            {
                error = message,
                statusCode = statusCode,
                // в Development-окружении можно включить стектрейс для отладки
                traceId = context.TraceIdentifier
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            await context.Response.WriteAsync(json);
        }
    }
}