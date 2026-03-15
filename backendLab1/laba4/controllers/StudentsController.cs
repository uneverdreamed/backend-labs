using laba4.models;
using Microsoft.AspNetCore.Mvc;

namespace laba4.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private static readonly List<Student> _students = new()
        {
            new Student { Id = 1, Name = "Иванов Иван", Group = "241-333", CreatedAt = DateTime.UtcNow,
            Courses = new List<Course>
                {
                    new Course { Id = 1, Name = "Backend-разработка", Slug = "back-13672", Year = 2020, Guid = Guid.NewGuid() },
                    new Course { Id = 2, Name = "Шаблоны проектирования", Slug = "despatterns-13377", Year = 2025, Guid = Guid.NewGuid() }
                }},
            new Student { Id = 2, Name = "Петров Петр", Group = "241-332", CreatedAt = DateTime.UtcNow,
            Courses = new List<Course>
                { new Course {Id = 3, Name = "Веб-программирование и дизайн", Slug = "webdev-13533", Year = 2019, Guid = Guid.NewGuid() }
            } },
            new Student { Id = 3, Name = "Николаев Николай", Group = "241-331", CreatedAt = DateTime.UtcNow,
            Courses = new List<Course>()
            }
        };
        
        private static int _nextId = 4;

        // GET получение всех студентов
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAll()
        {
            return Ok(_students);
        }

        // GET получение одного студента
        [HttpGet("{id}")]
        public ActionResult<Student> GetById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound(new { message = $"Студент с id={id} не найден" });

            return Ok(student);
        }


        // POST создание студента
        [HttpPost]
        public ActionResult<Student> Create([FromBody] CreateStudentDto dto)
        {
            var student = new Student
            {
                Id = _nextId++,
                Name = dto.Name,
                Group = dto.Group,
                CreatedAt = DateTime.UtcNow
            };

            _students.Add(student);

            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }


        // PUT обновление студента
        [HttpPut("{id}")]
        public ActionResult<Student> Update(int id, [FromBody] UpdateStudentDto dto)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound(new { message = $"Студент с id={id} не найден" });

            student.Name = dto.Name;
            student.Group = dto.Group;

            return Ok(student);
        }


        // DELETE удаление студента
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound(new { message = $"Студент с id={id} не найден" });

            _students.Remove(student);

            return NoContent();
        }
    }
}