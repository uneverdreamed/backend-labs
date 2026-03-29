namespace laba7.middleware
{
    public class BlockPathMiddleware
    {
        private readonly RequestDelegate _next;

        public BlockPathMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // проверка пути запроса (если начинается с /blocked, сразу возвращается 403)
            // next() не вызывается, конвейер прерывается здесь
            if (context.Request.Path.StartsWithSegments("/blocked"))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access denied: this path is blocked.");
                return;
            }
            // если путь не заблокирован, то управление передается дальше по конвейеру
            await _next(context);
        }
    }
}
