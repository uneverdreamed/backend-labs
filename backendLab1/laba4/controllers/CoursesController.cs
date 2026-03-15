using laba4.models;
using Microsoft.AspNetCore.Mvc;

namespace laba4.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private static readonly List<Course> _courses = new()
        {
            new Course { Id = 1, Name = "Backend-разработка", Slug = "back-13672", Year = 2020, Guid = Guid.NewGuid() },
            new Course { Id = 2, Name = "Шаблоны проектирования", Slug = "despatterns-13377", Year = 2025, Guid = Guid.NewGuid() },
            new Course { Id = 3, Name = "Веб-программирование и дизайн", Slug = "webdev-13533", Year = 2019, Guid = Guid.NewGuid() }
        };

        private static int _nextId = 4;

        // GET api/courses
        [HttpGet]
        public ActionResult GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sort = "id",
            [FromQuery] int? year = null)
        {
            var query = _courses.AsEnumerable();

            if (year.HasValue)
                query = query.Where(c => c.Year == year.Value);

            query = sort.ToLower() switch
            {
                "name" => query.OrderBy(c => c.Name),
                "year" => query.OrderBy(c => c.Year),
                _ => query.OrderBy(c => c.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new { page, pageSize, sort, year, totalCount = _courses.Count, data = result });
        }

        // GET api/courses/{id:int}
        [HttpGet("{id:int}")]
        public ActionResult<Course> GetById(int id)
        {
            var course = _courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound(new { message = $"Курс с id={id} не найден" });

            return Ok(course);
        }

        // GET api/courses/by-slug/{slug:minlength(5)}
        [HttpGet("by-slug/{slug:minlength(5)}")]
        public ActionResult<Course> GetBySlug(string slug)
        {
            var course = _courses.FirstOrDefault(c => c.Slug == slug);
            if (course == null)
                return NotFound(new { message = $"Курс со slug='{slug}' не найден" });

            return Ok(course);
        }

        // GET api/courses/by-year/{year:int}
        [HttpGet("by-year/{year:int}")]
        public ActionResult GetByYear(int year)
        {
            var result = _courses.Where(c => c.Year == year).ToList();
            return Ok(new { year, data = result });
        }

        // POST api/courses
        [HttpPost]
        public ActionResult<Course> Create([FromBody] Course course)
        {
            course.Id = _nextId++;
            course.Guid = Guid.NewGuid();
            _courses.Add(course);
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        // PUT api/courses/{id:int}
        [HttpPut("{id:int}")]
        public ActionResult<Course> Update(int id, [FromBody] Course updated)
        {
            var course = _courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound(new { message = $"Курс с id={id} не найден" });

            course.Name = updated.Name;
            course.Slug = updated.Slug;
            course.Year = updated.Year;
            return Ok(course);
        }

        // DELETE api/courses/{id:int}
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var course = _courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound(new { message = $"Курс с id={id} не найден" });

            _courses.Remove(course);
            return NoContent();
        }

    }
}
