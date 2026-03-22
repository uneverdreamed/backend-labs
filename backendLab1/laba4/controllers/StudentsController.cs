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
        public ActionResult<IEnumerable<Student>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sort = "id")
        {
            var query = _students.AsEnumerable();

            query = sort.ToLower() switch
            {
                "name" => query.OrderBy(s => s.Name),
                "group" => query.OrderBy(s => s.Group),
                _ => query.OrderBy(s => s.Id)
            };
            var result = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                page,
                pageSize,
                sort,
                totalCount = _students.Count,
                data = result
            });
        }

        // GET получение одного студента
        [HttpGet("{id:int}")]
        public ActionResult<Student> GetById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound(new { message = $"Студент с id={id} не найден" });

            return Ok(student);
        }

        // GET api/students/by-name/{name}
        [HttpGet("by-name/{name}")]
        public ActionResult<IEnumerable<Student>> GetByName(string name)
        {
            var result = _students
                .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!result.Any())
                return NotFound(new { message = $"Студенты с именем '{name}' не найдены" });

            return Ok(result);
        }

        // GET api/students/by-year/{year:int}
        [HttpGet("by-year/{year:int}")]
        public ActionResult<IEnumerable<Student>> GetByYear(int year)
        {
            var result = _students
                .Where(s => s.Courses.Any(c => c.Year == year))
                .ToList();

            return Ok(new { year, data = result });
        }

        // GET api/students/by-date/{date:datetime}
        [HttpGet("by-date/{date:datetime}")]
        public ActionResult GetByDate(DateTime date)
        {
            return Ok(new { message = $"Запрос студентов за дату: {date:yyyy-MM-dd}" });
        }

        // GET api/students/by-guid/{guid:guid}
        [HttpGet("by-guid/{guid:guid}")]
        public ActionResult GetByGuid(Guid guid)
        {
            var student = _students
                .FirstOrDefault(s => s.Courses.Any(c => c.Guid == guid));

            if (student == null)
                return NotFound(new { message = $"Студент с курсом guid={guid} не найден" });

            return Ok(student);
        }

        // GET api/students/by-slug/{slug}
        [HttpGet("by-slug/{slug:minlength(5)}")]
        public ActionResult GetBySlug(string slug)
        {
            var student = _students
                .FirstOrDefault(s => s.Courses.Any(c => c.Slug == slug));

            if (student == null)
                return NotFound(new { message = $"Курс со slug='{slug}' не найден" });

            return Ok(student);
        }

        // GET api/students/{id?}
        [HttpGet("optional/{id?}")]
        public ActionResult GetOptional(int? id = null)
        {
            if (id == null)
                return Ok(new { message = "id не передан, возврат всех студентов", data = _students });

            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound(new { message = $"Студент с id={id} не найден" });

            return Ok(student);
        }

        // GET api/students/{id:int}/courses
        [HttpGet("{id:int}/courses")]
        public ActionResult<IEnumerable<Course>> GetCourses(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound(new { message = $"Студент с id={id} не найден" });

            return Ok(new
            {
                studentId = id,
                studentName = student.Name,
                courses = student.Courses
            });
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


        // PUT api/students/{id:int}
        [HttpPut("{id:int}")]
        public ActionResult<Student> Update(int id, [FromBody] Student updated)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound(new { message = $"Студент с id={id} не найден" });

            student.Name = updated.Name;
            student.Group = updated.Group;
            return Ok(student);
        }


        // DELETE api/students/{id:int}
        [HttpDelete("{id:int}")]
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