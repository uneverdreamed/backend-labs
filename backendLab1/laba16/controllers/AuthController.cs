using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using laba16.Data;
using laba16.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace laba16.Controllers;

// Контроллер аутентификации - проверяет логин/пароль и выдаёт JWT-токен
// Этот контроллер открытый, на нём нет атрибута [Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    // IConfiguration внедряется автоматически, через него получаем настройки JWT
    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    // Эндпоинт входа - принимает логин/пароль, возвращает JWT-токен
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        // поиск пользователя по логину и паролю (в реальном приложении пароль был бы захеширован, и сравнение шло бы через хеш)
        var user = await _db.Users.FirstOrDefaultAsync(u =>
            u.Login == dto.Login && u.Password == dto.Password);

        // Если пользователь не найден - 401 Unauthorized с сообщением
        if (user == null)
        {
            return Unauthorized(new { error = "Неверный логин или пароль" });
        }

        // формирование токена и возврат клиенту
        var token = GenerateJwtToken(user.Id, user.Login, user.Role);

        return Ok(new
        {
            token,
            user = new { user.Id, user.Login, user.Role }
        });
    }

    // Формирование JWT-токена для пользователя
    private string GenerateJwtToken(int userId, string login, string role)
    {
        // Список клеймов с основными данными пользователя
        // Sub (subject) и Jti (token id) - стандартные клеймы из RFC 7519
        // NameIdentifier и Name - используются ASP.NET Core по умолчанию для User.Identity
        // Role - используется атрибутом [Authorize(Roles = "...")] для проверки доступа
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, login),
            new(ClaimTypes.Role, role)
        };

        // симметричный ключ для подписи токена - тот же, что используется при валидации в Program.cs HmacSha256
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Срок жизни токена 
        var expirationMinutes = int.Parse(_config["Jwt:ExpirationMinutes"]!);

        // Сборка токена - issuer и audience должны совпадать с настройками в Program.cs
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        // Сериализация токена в строку формата header.payload.signature
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}