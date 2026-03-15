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
            new Course { Id = 1, Name = "Backend-разработка", Slug = "back-13672", Year = 2024, Guid = Guid.NewGuid() },
            new Course { Id = 2, Name = "Шаблоны проектирования", Slug = "despatterns-13377", Year = 2025, Guid = Guid.NewGuid() },
            new Course { Id = 3, Name = "Веб-программирование и дизайн", Slug = "webdev-13533", Year = 2026, Guid = Guid.NewGuid() }
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
    }
}
