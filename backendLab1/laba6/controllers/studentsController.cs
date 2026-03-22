using laba6.interfaces;
using laba6.models;
using Microsoft.AspNetCore.Mvc;

namespace lab6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;
        public StudentsController(IStudentService service)
        {
            _service = service;
        }

        // GET api/students
        [HttpGet]
        public ActionResult<IEnumerable<StudentDto>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        // GET api/students/{id}
        [HttpGet("{id:int}")]
        public ActionResult<StudentDto> GetById(int id)
        {
            var student = _service.GetById(id);
            if (student == null)
                return NotFound(new { message = $"Студент с id={id} не найден" });

            return Ok(student);
        }

        // POST api/students
        [HttpPost]
        public ActionResult<StudentDto> Create([FromBody] CreateStudentDto dto)
        {
            var created = _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}