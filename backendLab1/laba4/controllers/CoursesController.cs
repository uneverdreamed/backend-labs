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
            new Course { Id = 13672, Name = "Backend-разработка", Slug = "back-1372", Year = 2024, Guid = Guid.NewGuid() },
            new Course { Id = 13377, Name = "Шаблоны проектирования", Slug = "despatterns-1377", Year = 2025, Guid = Guid.NewGuid() },
            new Course { Id = 13533, Name = "Веб-программирование и дизайн", Slug = "webdev-1333", Year = 2026, Guid = Guid.NewGuid() }
        };  
    }
}
