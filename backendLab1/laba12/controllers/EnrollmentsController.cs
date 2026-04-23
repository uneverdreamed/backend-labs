using laba12.data;
using laba12.DTOs;
using laba12.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace laba12.controllers
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enroll>>> GetAll()
        {
            var enrollments = await _context.Enrolls.ToListAsync();
            return Ok(enrollments);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Enroll>> GetById(int id)
        {
            var enrollment = await _context.Enrolls.FindAsync(id);

            if (enrollment == null)
                return NotFound(new { error = $"Запись с id={id} не найдена", statusCode = 404 });

            return Ok(enrollment);
        }

        [HttpPost]
        public async Task<ActionResult<Enroll>> Create([FromBody] CreateEnrollDto dto)
        {
            // проверяем существование студента и курса через ModelState
            var studentExists = await _context.Students.AnyAsync(s => s.Id == dto.StudentId);
            if (!studentExists)
                ModelState.AddModelError("StudentId", $"Студент с id={dto.StudentId} не найден");

            var courseExists = await _context.Courses.AnyAsync(c => c.Id == dto.CourseId);
            if (!courseExists)
                ModelState.AddModelError("CourseId", $"Курс с id={dto.CourseId} не найден");

            // проверяем, не записан ли студент уже на этот курс
            if (studentExists && courseExists)
            {
                var alreadyEnrolled = await _context.Enrolls
                    .AnyAsync(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId);
                if (alreadyEnrolled)
                    ModelState.AddModelError("", $"Студент уже записан на этот курс");
            }

            // если есть хотя бы одна ошибка — возвращаем все сразу
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

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

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Enroll>> Update(int id, [FromBody] UpdateEnrollDto dto)
        {
            var enrollment = await _context.Enrolls.FindAsync(id);

            if (enrollment == null)
                return NotFound(new { error = $"Запись с id={id} не найдена", statusCode = 404 });

            enrollment.Grade = dto.Grade;

            await _context.SaveChangesAsync();
            return Ok(enrollment);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var enrollment = await _context.Enrolls.FindAsync(id);

            if (enrollment == null)
                return NotFound(new { error = $"Запись с id={id} не найдена", statusCode = 404 });

            _context.Enrolls.Remove(enrollment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}