using laba11.data;
using laba11.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab11.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        // AppDbContext внедряется через DI, EF Core регистрирует его как Scoped-сервис
        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/students - получить всех студентов со списком их курсов
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            // Include загружает связанные данные (JOIN в SQL), ThenInclude — загружает данные второго уровня вложенности
            var students = await _context.Students
                .Include(s => s.Enrolls)
                    .ThenInclude(e => e.Course)
                .ToListAsync();

            return Ok(students);
        }

        // GET api/students/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            var student = await _context.Students
                .Include(s => s.Enrolls)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound(new { message = $"Студент с id={id} не найден" });

            return Ok(student);
        }

        // POST api/students — создать студента
        [HttpPost]
        public async Task<ActionResult<Student>> Create([FromBody] Student student)
        {
            student.CreatedAt = DateTime.UtcNow;

            // Add — добавляет сущность в контекст (состояние Added)
            _context.Students.Add(student);
            // SaveChangesAsync — генерирует INSERT и выполняет его в базе
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        // PUT api/students/{id} — обновить студента
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Student>> Update(int id, [FromBody] Student updated)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound(new { message = $"Студент с id={id} не найден" });

            student.Name = updated.Name;
            student.Group = updated.Group;

            // EF Core отслеживает изменения — SaveChangesAsync сгенерирует UPDATE
            await _context.SaveChangesAsync();

            return Ok(student);
        }

        // DELETE api/students/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound(new { message = $"Студент с id={id} не найден" });

            // Remove — помечает сущность для удаления, SaveChangesAsync выполнит DELETE
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}