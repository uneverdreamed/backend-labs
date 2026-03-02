namespace laba4.models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
