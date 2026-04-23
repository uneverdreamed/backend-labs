using laba12.data;
using laba12.DTOs;
using laba12.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace laba12.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAll()
        {
            var courses = await _context.Courses.ToListAsync();
            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Course>> GetById(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound(new { error = $"Курс с id={id} не найден", statusCode = 404 });

            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult<Course>> Create([FromBody] CreateCourseDto dto)
        {
            // проверка бизнес-логики: название курса должно быть уникальным
            var duplicate = await _context.Courses.AnyAsync(c => c.Name == dto.Name);
            if (duplicate)
            {
                ModelState.AddModelError("Name", $"Курс '{dto.Name}' уже существует");
                return ValidationProblem(ModelState);
            }

            var course = new Course
            {
                Name = dto.Name,
                Description = dto.Description,
                Credits = dto.Credits
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Course>> Update(int id, [FromBody] CreateCourseDto dto)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound(new { error = $"Курс с id={id} не найден", statusCode = 404 });

            course.Name = dto.Name;
            course.Description = dto.Description;
            course.Credits = dto.Credits;

            await _context.SaveChangesAsync();
            return Ok(course);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound(new { error = $"Курс с id={id} не найден", statusCode = 404 });

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}