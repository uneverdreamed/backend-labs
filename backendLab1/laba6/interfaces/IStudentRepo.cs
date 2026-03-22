using laba6.models;

namespace laba6.interfaces
{
    public interface IStudentRepo
    {
        IEnumerable<student> GetAll();
        student? GetById(int id);
        student Add(student student);
    }
}
