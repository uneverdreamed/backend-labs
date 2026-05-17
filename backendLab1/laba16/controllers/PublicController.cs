using Microsoft.AspNetCore.Mvc;

namespace laba16.Controllers;

// Открытый контроллер - доступен без токена
// Используется для проверки, что эндпоинты без [Authorize] работают для всех
[ApiController]
[Route("api/[controller]")]
public class PublicController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "Это открытый эндпоинт, токен не требуется",
            time = DateTime.Now
        });
    }
}