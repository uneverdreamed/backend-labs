using laba6.models;

namespace laba6.interfaces
{
    public interface IStudentRepo
    {
        IEnumerable<Student> GetAll();
        Student? GetById(int id);
        Student Add(Student student);
    }
}
