using laba6.models;

namespace laba6.interfaces
{
    public interface IStudentService
    {
        IEnumerable<StudentDto> GetAll();
        StudentDto? GetById(int id);
        StudentDto Create(CreateStudentDto dto);
    }
}
