using laba6.models;
using laba6.interfaces;

namespace laba6.services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo _repository;
        public StudentService(IStudentRepo repository)
        {
            _repository = repository;
        }

        public IEnumerable<StudentDto> GetAll()
        {
            return _repository.GetAll().Select(MapToDto);
        }

        public StudentDto? GetById(int id)
        {
            var student = _repository.GetById(id);
            return student == null ? null : MapToDto(student);
        }

        public StudentDto Create(CreateStudentDto dto)
        {
            var student = new Student
            {
                Name = dto.Name,
                Group = dto.Group
                // Id и CreatedAt устанавливаются в репозитории
            };

            var created = _repository.Add(student);
            return MapToDto(created);
        }

        private static StudentDto MapToDto(Student student) => new StudentDto
        {
            Id = student.Id,
            Name = student.Name,
            Group = student.Group,
            CreatedAt = student.CreatedAt
        };
    }
}
