using laba15.Models;

var builder = WebApplication.CreateBuilder(args);

// регистрация контроллеров с поддержкой представлений (только API-контроллеры)
builder.Services.AddControllers();

// подключение поддержки серверных сессий
// использование хранилища в памяти процесса
builder.Services.AddDistributedMemoryCache();

// найстройка параметров сессии
builder.Services.AddSession(options =>
{
    // Имя cookie, через которую браузер передаёт идентификатор сессии
    options.Cookie.Name = "Laba15.Session";
    // Время бездействия, после которого сессия считается истёкшей
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    // Запрет доступа к cookie из JavaScript для защиты от XSS
    options.Cookie.HttpOnly = true;
    // Cookie сессии помечается как обязательная и не подпадает под согласие на cookies
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Включаем работу со статическими файлами из wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

// Сессии должны быть подключены между UseRouting и MapControllers
app.UseSession();

app.MapControllers();

app.Run();