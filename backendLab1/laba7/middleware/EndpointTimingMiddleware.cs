using System.Diagnostics;

namespace laba7.middleware
{
    public class EndpointTimingMiddleware
    {
        private readonly RequestDelegate _next;

        public EndpointTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew(); // запуск таймера до передачи управления следующему компоненту


            // все что находится ДО next() выполняется при входе запроса
            await _next(context);
            // все что находится ПОСЛЕ next() выполняется когда ответ уже сформирован
            stopwatch.Stop();

            // добавление заголовка с временем выполнения в миллисекундах
            // используется TryAdd (если заголовок уже отправлен, не выбрасывается исключение)
            context.Response.Headers.TryAdd("X-Endpoint-Elapsed-Ms", stopwatch.ElapsedMilliseconds.ToString());
        }
    }
}
