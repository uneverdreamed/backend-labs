using lab2;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
    options.SerializerOptions.WriteIndented = true;
});

var app = builder.Build();

var users = new List<User>
{
    new User { Id = 1, Name = "Имя1", Age = 21, Email = "imya1@pochta.com"},
    new User { Id = 2, Name = "Имя2", Age = 31, Email = "imya2@pochta.com"},
    new User { Id = 3, Name = "Имя3", Age = 41, Email = "imya3@pochta.com"}

};

// get 
app.MapGet("api/users", () =>
{
    return Results.Ok(new
    {
        message = "Список всех пользователей",
        count = users.Count,
        data = users
    });
});

app.MapGet("/api/users/search", (int? id) =>
{
    if (id == null)
    {
        return Results.BadRequest(new { message = "Параметр id не указан" });
    }

    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
    {
        return Results.NotFound(new { message = $"Пользователь с id {id} не найден" });
    }

    return Results.Ok(new
    {
        message = $"Пользователь с id {id} найден",
        data = user
    });
});

// post
app.MapPost("/api/users", (User user) =>
{
    if (string.IsNullOrWhiteSpace(user.Name))
    {
        return Results.BadRequest(new { error = "Имя обязательно" });
    }

    if (user.Age <= 0)
    {
        return Results.BadRequest(new { error = "Возраст должен быть положительным" });
    }

    user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;

    users.Add(user);

    return Results.Created($"/api/users/{user.Id}", new
    {
        message = "Пользователь успешно создан",
        data = user
    });
});

// post с query и body

app.MapPost("/api/users/register", (string category, User user) =>
{
    if (string.IsNullOrWhiteSpace(category))
    {
        return Results.BadRequest(new { error = "Параметр category обязателен" });
    }

    if (string.IsNullOrWhiteSpace(user.Name))
    {
        return Results.BadRequest(new { error = "Имя обязательно" });
    }

    user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
    users.Add(user);

    return Results.Created($"/api/users/{user.Id}", new
    {
        message = $"Пользователь зарегистрирован в категории '{category}'",
        category = category,
        data = user
    });
});

// put
app.MapPut("api/users/{id}", (int id, User updatedUser) =>
{ 
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
    {
        return Results.NotFound(new {error = $"Пользователь с id {id} не найден" });
    }

    if (string.IsNullOrWhiteSpace(updatedUser.Name))
    {
        return Results.BadRequest(new { error = "Имя обязательно" });
    }
    user.Name = updatedUser.Name;
    user.Age = updatedUser.Age;
    user.Email = updatedUser.Email;

    return Results.Ok(new
    {
        message = "Пользователь полностью обновлен",
        data = user
    });
});


// patch
app.MapPatch("/api/users/{id}", (int id, UserPatch patch) =>
{
var user = users.FirstOrDefault(u => u.Id == id);

if (user == null)
{
    return Results.NotFound(new { error = $"Пользователь с id={id} не найден" });
}

    if (patch.Name != null)
    {
        user.Name = patch.Name;
    }

    if (patch.Age.HasValue)
    {
        user.Age = patch.Age.Value;
    }

    if (patch.Email != null)
    {
        user.Email = patch.Email;
    }

    return Results.Ok(new
    {
        message = "Пользователь частично обновлен",
        updatedFields = new
        {
            name = patch.Name != null,
            age = patch.Age.HasValue,
            email = patch.Email != null
        },
        data = user
    });
});

app.Run();
