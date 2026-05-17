using System.Text;
using laba16.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// регистрация контроллеров для маршрутизации API-запросов
builder.Services.AddControllers();

// контекст базы данных SQLite - хранит пользователей и учебные сущности
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=laba16.db"));

// чтение настроек JWT из appsettings.json в переменные
// они нужны и при регистрации аутентификации, и при выдаче токена в контроллере
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("Не задан Jwt:Issuer в appsettings.json");
var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("Не задан Jwt:Audience в appsettings.json");
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Не задан Jwt:Key в appsettings.json");

// Регистрация схемы аутентификации JWT Bearer
// AddAuthentication задаёт схему по умолчанию, которой будут пользоваться [Authorize]
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // TokenValidationParameters описывает, какие проверки делать при валидации входящего токена
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Проверять, что токен выдан нашим сервером (значение совпадает с iss в токене)
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            // Проверять, что токен предназначен нашему клиенту (значение совпадает с aud в токене)
            ValidateAudience = true,
            ValidAudience = jwtAudience,

            // Проверять срок жизни токена - после exp токен считается невалидным
            ValidateLifetime = true,

            // Проверять, что подпись токена сделана нашим секретным ключом
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            // Убираем стандартный запас времени в 5 минут, чтобы exp срабатывал точно
            ClockSkew = TimeSpan.Zero
        };
    });

// Включаем сервис авторизации - именно он обрабатывает атрибут [Authorize(Roles = "...")]
builder.Services.AddAuthorization();

// Swagger с поддержкой авторизации через кнопку Authorize в UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Laba16 API", Version = "v1" });

    // Описание схемы безопасности - Bearer-токен в заголовке Authorization
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT-токен в формате: Bearer {токен}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Требование - все защищённые эндпоинты должны принимать схему Bearer
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Пересоздаём базу при каждом старте и заполняем начальными данными
// Подход из предыдущих лабораторных, чтобы не возиться с миграциями
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();
// Редирект с корня на Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

// Порядок middleware важен: сначала аутентификация (кто это?), потом авторизация (что ему можно?)
// Аутентификация читает токен из заголовка и заполняет HttpContext.User
// Авторизация проверяет атрибуты [Authorize] на основе HttpContext.User
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();