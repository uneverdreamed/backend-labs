using laba16.Data;
using laba16.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace laba16.Controllers;

// Контроллер студентов с разными уровнями доступа на разных методах
// Атрибут [Authorize] на уровне контроллера требует авторизации для всех методов но отдельные методы могут переопределять это требование
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public StudentsController(AppDbContext db)
    {
        _db = db;
    }

    // Чтение списка студентов - доступно любому авторизованному пользователю
    // Достаточно просто иметь валидный токен, роль не проверяется
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _db.Students.ToListAsync();
        return Ok(students);
    }

    // Получение одного студента - тоже для всех авторизованных
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student == null) return NotFound();
        return Ok(student);
    }

    // Создание студента - только для ролей Admin и Manager
    // Через запятую перечисляются роли, которым разрешён доступ (логика "или")
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] Student student)
    {
        _db.Students.Add(student);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
    }

    // Удаление студента - только для роли Admin
    // Manager и обычный User получат 403 Forbidden при попытке вызова
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student == null) return NotFound();

        _db.Students.Remove(student);
        await _db.SaveChangesAsync();
        return Ok(new { message = $"Студент {student.Name} удалён" });
    }
}