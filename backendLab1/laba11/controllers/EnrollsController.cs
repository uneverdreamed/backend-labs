using laba11.data;
using laba11.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab11.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnrollmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/enrollments — все записи с данными студентов и курсов
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enroll>>> GetAll()
        {
            var enrollments = await _context.Enrolls
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();

            return Ok(enrollments);
        }

        // GET api/enrollments/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Enroll>> GetById(int id)
        {
            var enrollment = await _context.Enrolls
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enrollment == null)
                return NotFound(new { message = $"Запись с id={id} не найдена" });

            return Ok(enrollment);
        }

        // POST api/enrollments — записать студента на курс
        [HttpPost]
        public async Task<ActionResult<Enroll>> Create([FromBody] Enroll enrollment)
        {
            // Проверяем существование студента и курса перед созданием записи
            var studentExists = await _context.Students.AnyAsync(s => s.Id == enrollment.StudentId);
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == enrollment.CourseId);

            if (!studentExists)
                return BadRequest(new { message = $"Студент с id={enrollment.StudentId} не найден" });

            if (!courseExists)
                return BadRequest(new { message = $"Курс с id={enrollment.CourseId} не найден" });

            enrollment.EnrolledAt = DateTime.UtcNow;
            _context.Enrolls.Add(enrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = enrollment.Id }, enrollment);
        }

        // PUT api/enrollments/{id} — обновить оценку
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Enroll>> Update(int id, [FromBody] Enroll updated)
        {
            var enrollment = await _context.Enrolls.FindAsync(id);

            if (enrollment == null)
                return NotFound(new { message = $"Запись с id={id} не найдена" });

            enrollment.Grade = updated.Grade;

            await _context.SaveChangesAsync();
            return Ok(enrollment);
        }

        // DELETE api/enrollments/{id} — отчислить студента с курса
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var enrollment = await _context.Enrolls.FindAsync(id);

            if (enrollment == null)
                return NotFound(new { message = $"Запись с id={id} не найдена" });

            _context.Enrolls.Remove(enrollment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}