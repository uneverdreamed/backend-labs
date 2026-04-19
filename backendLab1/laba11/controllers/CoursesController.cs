using laba11.data;
using laba11.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab11.Controllers
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

        // GET api/courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAll()
        {
            var courses = await _context.Courses
                .Include(c => c.Enrolls)
                    .ThenInclude(e => e.Student)
                .ToListAsync();

            return Ok(courses);
        }

        // GET api/courses/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Course>> GetById(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Enrolls)
                    .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound(new { message = $"Курс с id={id} не найден" });

            return Ok(course);
        }

        // POST api/courses
        [HttpPost]
        public async Task<ActionResult<Course>> Create([FromBody] Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        // PUT api/courses/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Course>> Update(int id, [FromBody] Course updated)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound(new { message = $"Курс с id={id} не найден" });

            course.Name = updated.Name;
            course.Description = updated.Description;
            course.Credits = updated.Credits;

            await _context.SaveChangesAsync();
            return Ok(course);
        }

        // DELETE api/courses/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound(new { message = $"Курс с id={id} не найден" });

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}