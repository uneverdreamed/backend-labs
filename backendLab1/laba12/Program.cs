using laba12.data;
using laba12.filters;
using laba12.middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// регистрация контроллеров с настройкой JSON-сериализации
builder.Services.AddControllers(options =>
{
    // регистрация DbExceptionFilter глобально — он будет применяться ко всем контроллерам
    // используется ServiceFilter, чтобы фильтр получал зависимости через DI (ILogger)
    options.Filters.Add<DbExceptionFilter>();
})
.AddJsonOptions(options =>
{
    // игнорировать циклические ссылки при сериализации
    options.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// регистрация DbExceptionFilter как сервиса — нужно для внедрения ILogger через DI
builder.Services.AddScoped<DbExceptionFilter>();

// регистрация AppDbContext с провайдером SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// GlobalExceptionMiddleware подключается первым — он оборачивает весь конвейер
// и ловит любые исключения, которые не были обработаны фильтрами
app.UseMiddleware<GlobalExceptionMiddleware>();

// автоматическое применение миграций при старте
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();