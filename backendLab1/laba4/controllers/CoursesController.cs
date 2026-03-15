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
    }
}
