using laba11.data;
using laba11.Models;
using laba11.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace laba11.controllers
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

        // GET api/enrolls — все записи с данными студентов и курсов
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
        public async Task<ActionResult<Enroll>> Create([FromBody] CreateEnrollDto dto)
        {
            var studentExists = await _context.Students.AnyAsync(s => s.Id == dto.StudentId);
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == dto.CourseId);
            if (!studentExists)
                return BadRequest(new { message = $"Студент с id={dto.StudentId} не найден" });
            if (!courseExists)
                return BadRequest(new { message = $"Курс с id={dto.CourseId} не найден" });
            var enrollment = new Enroll
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                Grade = dto.Grade,
                EnrolledAt = DateTime.UtcNow
            };
            _context.Enrolls.Add(enrollment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = enrollment.Id }, enrollment);
        }

        // PUT api/enrollments/{id} — обновить оценку
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Enroll>> Update(int id, [FromBody] UpdateEnrollDto dto)
        {
            var enrollment = await _context.Enrolls.FindAsync(id);
            if (enrollment == null)
                return NotFound(new { message = $"Запись с id={id} не найдена" });
            enrollment.Grade = dto.Grade;
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