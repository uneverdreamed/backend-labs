using laba12.data;
using laba12.DTOs;
using laba12.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace laba12.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }

        // GET api/students/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound(new { error = $"Студент с id={id} не найден", statusCode = 404 });

            return Ok(student);
        }

        // POST api/students
        // атрибуты валидации на DTO проверяются автоматически благодаря [ApiController]
        // если ModelState невалиден, ASP.NET Core вернёт 400 с деталями ошибок
        [HttpPost]
        public async Task<ActionResult<Student>> Create([FromBody] CreateStudentDto dto)
        {
            // пример ручной проверки бизнес-логики через ModelState:
            // проверяем, нет ли уже студента с таким именем в той же группе
            var duplicate = await _context.Students
                .AnyAsync(s => s.Name == dto.Name && s.Group == dto.Group);

            if (duplicate)
            {
                // добавляем ошибку в ModelState — она вернётся клиенту в формате ValidationProblem
                ModelState.AddModelError("Name", $"Студент '{dto.Name}' уже существует в группе {dto.Group}");
                return ValidationProblem(ModelState);
            }

            var student = new Student
            {
                Name = dto.Name,
                Group = dto.Group,
                CreatedAt = DateTime.UtcNow
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        // PUT api/students/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Student>> Update(int id, [FromBody] CreateStudentDto dto)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound(new { error = $"Студент с id={id} не найден", statusCode = 404 });

            // проверяем дубликат при обновлении (исключая текущую запись)
            var duplicate = await _context.Students
                .AnyAsync(s => s.Name == dto.Name && s.Group == dto.Group && s.Id != id);

            if (duplicate)
            {
                ModelState.AddModelError("Name", $"Студент '{dto.Name}' уже существует в группе {dto.Group}");
                return ValidationProblem(ModelState);
            }

            student.Name = dto.Name;
            student.Group = dto.Group;

            await _context.SaveChangesAsync();
            return Ok(student);
        }

        // DELETE api/students/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound(new { error = $"Студент с id={id} не найден", statusCode = 404 });

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}