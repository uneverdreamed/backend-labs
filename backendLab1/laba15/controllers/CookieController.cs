using laba15.Models;
using Microsoft.AspNetCore.Mvc;

namespace laba15.Controllers;

// Контроллер для работы с cookies на стороне сервера
// Демонстрирует создание, чтение и удаление cookie через HTTP-заголовки
[ApiController]
[Route("api/[controller]")]
public class CookieController : ControllerBase
{
    // Ключи cookies вынесены в константы, чтобы избежать опечаток при чтении и записи
    private const string CookieName = "UserName";
    private const string CookieGroup = "UserGroup";

    // Сохранение данных пользователя в cookies
    // Принимает JSON-объект, кладёт значения в cookies со сроком жизни 7 дней
    [HttpPost("save")]
    public IActionResult Save([FromBody] UserData data)
    {
        // Настройки cookie: срок жизни 7 дней, доступ только по HTTP (не из JS)
        // IsEssential = true помечает cookie как необходимую для работы приложения
        var options = new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            IsEssential = true
        };

        // Response.Cookies.Append добавляет заголовок Set-Cookie в HTTP-ответ
        // Браузер сам сохранит эти cookies и будет отправлять их в последующих запросах
        Response.Cookies.Append(CookieName, data.Name ?? string.Empty, options);
        Response.Cookies.Append(CookieGroup, data.Group ?? string.Empty, options);

        return Ok(new { message = "Данные сохранены в cookies", data });
    }

    // Чтение данных из cookies, переданных браузером в текущем запросе
    [HttpGet("read")]
    public IActionResult Read()
    {
        // Request.Cookies содержит все cookies, отправленные браузером
        // TryGetValue возвращает false, если cookie с таким именем не существует
        var hasName = Request.Cookies.TryGetValue(CookieName, out var name);
        var hasGroup = Request.Cookies.TryGetValue(CookieGroup, out var group);

        // Если ни одна cookie не найдена, сообщеине клиенту об отсутствии данных
        if (!hasName && !hasGroup)
        {
            return Ok(new { exists = false, message = "Cookies не найдены" });
        }

        return Ok(new
        {
            exists = true,
            name = name ?? string.Empty,
            group = group ?? string.Empty
        });
    }

    // Удаление cookies через специальный метод Delete
    // Технически отправляется Set-Cookie с истёкшей датой, и браузер удаляет cookie
    [HttpDelete("clear")]
    public IActionResult Clear()
    {
        Response.Cookies.Delete(CookieName);
        Response.Cookies.Delete(CookieGroup);
        return Ok(new { message = "Cookies удалены" });
    }
}