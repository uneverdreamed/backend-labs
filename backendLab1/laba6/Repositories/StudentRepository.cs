using laba6.interfaces;
using laba6.models;

namespace laba6.Repositories
{
    public class StudentRepository : IStudentRepo
    {
        private static readonly List<Student> _students = new()
        {
            new Student { Id = 1, Name = "Иванов Иван", Group = "241-331", CreatedAt = DateTime.UtcNow },
            new Student { Id = 2, Name = "Мариева Мария", Group = "241-332", CreatedAt = DateTime.UtcNow },
            new Student { Id = 3, Name = "Алексеев Алексей", Group = "241-333", CreatedAt = DateTime.UtcNow },
            new Student { Id = 4, Name = "Полинова Полина", Group = "241-333", CreatedAt= DateTime.UtcNow }
        };

        private static int _nextId = 5;

        public IEnumerable<Student> GetAll()
        {
            return _students;
        }

        public Student? GetById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        public Student Add(Student student)
        {
            student.Id = _nextId++;
            student.CreatedAt = DateTime.UtcNow;
            _students.Add(student);
            return student;
        }
    }
}