using laba15.Models;
using Microsoft.AspNetCore.Mvc;

namespace laba15.Controllers;

// Контроллер для пятой задачи - передача данных с клиента на backend
// Принимает данные из localStorage браузера и возвращает обогащённый ответ
// Демонстрирует, как клиентское хранилище может работать совместно с серверной частью
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    // Принимает данные пользователя, "обрабатывает" их на сервере и возвращает обратно
    [HttpPost("sync")]
    public IActionResult Sync([FromBody] UserData data)
    {
        // имитация серверной обработки (формирование приветствия и метки времени, которые сложно или нежелательно генерировать на клиенте)
        var greeting = string.IsNullOrWhiteSpace(data.Name)
            ? "hallooooo"
            : $"hallooooo, {data.Name} из группы {data.Group}!";

        return Ok(new
        {
            greeting,
            serverTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            receivedFrom = "localStorage клиента"
        });
    }
}