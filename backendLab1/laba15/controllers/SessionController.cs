using laba15.Models;
using Microsoft.AspNetCore.Mvc;

namespace laba15.Controllers;

// Контроллер для работы с серверными сессиями
// В отличие от cookies, данные хранятся на сервере, а клиент получает только идентификатор сессии
[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    // Ключи для значений в сессии
    private const string KeyName = "UserName";
    private const string KeyGroup = "UserGroup";
    private const string KeyVisits = "VisitCount";

    // Сохранение данных в сессию
    // HttpContext.Session доступен только после вызова app.UseSession() в Program.cs
    [HttpPost("save")]
    public IActionResult Save([FromBody] UserData data)
    {
        // SetString сохраняет строку в сессии под указанным ключом
        // Под капотом данные хранятся в IDistributedCache (у нас - в памяти процесса)
        HttpContext.Session.SetString(KeyName, data.Name ?? string.Empty);
        HttpContext.Session.SetString(KeyGroup, data.Group ?? string.Empty);

        return Ok(new { message = "Данные сохранены в сессии", data });
    }

    // Чтение данных из сессии и подсчёт количества обращений
    // При каждом GET-запросе увеличиваем счётчик визитов - это демонстрирует персистентность сессии между запросами одного клиента
    [HttpGet("read")]
    public IActionResult Read()
    {
        var name = HttpContext.Session.GetString(KeyName);
        var group = HttpContext.Session.GetString(KeyGroup);

        // GetInt32 возвращает nullable int - null, если ключа нет
        // ?? 0 даёт начальное значение для первого запроса
        var visits = HttpContext.Session.GetInt32(KeyVisits) ?? 0;
        visits++;
        HttpContext.Session.SetInt32(KeyVisits, visits);

        if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(group))
        {
            return Ok(new
            {
                exists = false,
                visits,
                message = "В сессии нет данных пользователя"
            });
        }

        return Ok(new
        {
            exists = true,
            name = name ?? string.Empty,
            group = group ?? string.Empty,
            visits
        });
    }

    // Очистка сессии - удаляет все данные, связанные с текущей сессией
    // Сам идентификатор сессии остаётся, но значения внутри сбрасываются
    [HttpDelete("clear")]
    public IActionResult Clear()
    {
        HttpContext.Session.Clear();
        return Ok(new { message = "Сессия очищена" });
    }
}
