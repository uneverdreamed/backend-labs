using laba14.data;
using laba14.models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace laba14.controllers
{
    // контроллер с политикой "ReadOnly" — только GET с любого источника
    // демонстрирует ограничительную CORS-политику: POST/PUT/DELETE будут заблокированы
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("ReadOnly")]
    public class PublicCoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PublicCoursesController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/publiccourses — разрешён политикой ReadOnly (GET с любого домена)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAll()
        {
            var courses = await _context.Courses.ToListAsync();
            return Ok(courses);
        }

        // POST api/publiccourses — будет заблокирован CORS при кросс-доменном запросе,
        // потому что политика "ReadOnly" разрешает только GET
        [HttpPost]
        public async Task<ActionResult<Course>> Create([FromBody] Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return Ok(course);
        }
    }
}