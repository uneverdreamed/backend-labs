using laba13.data;
using laba13.filters;
using laba13.middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// регистраци€ контроллеров с глобальным фильтром ошибок Ѕƒ
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

var app = builder.Build();

// глобальна€ обработка необработанных исключений
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

// UseDefaultFiles Ч при запросе к корню "/" подставл€ет index.html из wwwroot, должен идти ƒќ UseStaticFiles, иначе не сработает
app.UseDefaultFiles();

// UseStaticFiles Ч раздаЄт файлы из папки wwwroot (CSS, JS, HTML, изображени€) без этой строки сервер не будет отдавать статические файлы
app.UseStaticFiles();

app.MapControllers();
app.Run();