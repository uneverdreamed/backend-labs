namespace laba16.DTO;

// DTO для приёма данных при попытке входа
// Используется отдельно от модели User, чтобы не передавать через API лишних полей
public class LoginDto
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}