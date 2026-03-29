namespace laba7.middleware
{
    public class RequestTraceMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestTraceMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var traceId = Guid.NewGuid().ToString(); // генерация уникального TraceId для каждого запроса


            // сохранение TraceId в HttpContext.Items, чтобы он был доступен всем middleware и эндпоинтам в рамках запроса
            context.Items["TraceId"] = traceId;

            context.Response.Headers["X-Trace-Id"] = traceId; // добавление TraceId в заголовок ответа, чтобы клиент мог его увидеть

            await _next(context); // передача управления следующему компоненту конвейера

        }
    }
}
