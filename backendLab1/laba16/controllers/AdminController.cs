using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace laba16.Controllers;

// Контроллер только для администраторов
// Атрибут [Authorize(Roles = "Admin")] на уровне класса применяется ко всем методам
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    // Возвращает данные текущего пользователя, извлечённые из JWT-токенa ASP.NET Core автоматически парсит токен и заполняет HttpContext.User
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        // FindFirst ищет клейм по типу и возвращает его значение
        // Если клейма нет - возвращает null, поэтому используем оператор ?.
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var login = User.FindFirst(ClaimTypes.Name)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            message = "Доступ только для администраторов",
            userId,
            login,
            role
        });
    }
}