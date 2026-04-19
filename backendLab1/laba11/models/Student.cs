namespace laba11.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public ICollection<Enroll> Enrolls { get; set; } = new List<Enroll>();
    }
}
