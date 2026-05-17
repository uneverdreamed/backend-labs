namespace laba16.Models;

// Модель пользователя для аутентификации
// В реальном приложении пароль никогда не хранится в открытом виде
public class User
{
    public int Id { get; set; }

    // Логин - уникальный идентификатор пользователя при входе
    public string Login { get; set; } = string.Empty;

    // Пароль в открытом виде (учебный пример)
    // В продакшене - только хеш с солью через BCrypt или PBKDF2
    public string Password { get; set; } = string.Empty;

    // Роль определяет уровень доступа: Admin, Manager, User
    // Используется в [Authorize(Roles = "...")] для ограничения доступа
    public string Role { get; set; } = "User";
}