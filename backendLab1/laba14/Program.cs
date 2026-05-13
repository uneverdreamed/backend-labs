using laba14.data;
using laba14.filters;
using laba14.middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// регистрация контроллеров с глобальным фильтром ошибок БД
builder.Services.AddControllers(options =>
{
    options.Filters.Add<DbExceptionFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DbExceptionFilter>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== НАСТРОЙКА CORS =====
// AddCors регистрирует сервис CORS в DI-контейнере
// внутри определяются именованные политики с разными уровнями доступа
builder.Services.AddCors(options =>
{
    // Политика 1: "AllowClient" — разрешает запросы только от нашего клиента (порт 5001)
    // это основная рабочая политика для фронтенда
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins("http://localhost:5001")  // конкретный источник
              .AllowAnyMethod()                       // разрешены все HTTP-методы (GET, POST, PUT, DELETE)
              .AllowAnyHeader();                      // разрешены любые заголовки (Content-Type и др.)
    });

    // Политика 2: "ReadOnly" — разрешает только чтение (GET) с любого источника
    // пример ограничительной политики для публичных данных
    options.AddPolicy("ReadOnly", policy =>
    {
        policy.AllowAnyOrigin()
              .WithMethods("GET")
              .WithHeaders("Accept");
    });

    // Политика 3: "AllowAll" — разрешает всё (для разработки)
    // небезопасна для продакшена, но удобна при отладке
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// автоматическое создание базы данных при старте
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// UseCors подключает middleware обработки CORS-заголовков
// "AllowClient" — политика по умолчанию, применяется ко всем эндпоинтам
// на отдельных контроллерах/методах можно переопределить через атрибут [EnableCors("ИмяПолитики")]
app.UseCors("AllowClient");

app.MapControllers();
app.Run();