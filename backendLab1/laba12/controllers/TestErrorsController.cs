using Microsoft.AspNetCore.Mvc;

namespace laba12.controllers
{
    // тестовый контроллер — намеренно выбрасывает исключения разных типов,
    // чтобы продемонстрировать работу GlobalExceptionMiddleware
    [ApiController]
    [Route("api/[controller]")]
    public class TestErrorsController : ControllerBase
    {
        // GET api/testerrors/500 — необработанное исключение → 500
        [HttpGet("500")]
        public IActionResult TestInternalError()
        {
            throw new Exception("Тестовая непредвиденная ошибка сервера");
        }

        // GET api/testerrors/400 — ArgumentException → 400
        [HttpGet("400")]
        public IActionResult TestBadRequest()
        {
            throw new ArgumentException("Тестовая ошибка: неверный аргумент");
        }

        // GET api/testerrors/404 — KeyNotFoundException → 404
        [HttpGet("404")]
        public IActionResult TestNotFound()
        {
            throw new KeyNotFoundException("Тестовая ошибка: запись не найдена");
        }

        // GET api/testerrors/422 — InvalidOperationException → 422
        [HttpGet("422")]
        public IActionResult TestUnprocessable()
        {
            throw new InvalidOperationException("Тестовая ошибка: операция невозможна");
        }
    }
}