namespace laba4.models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty; 
        public int Year { get; set; }
        public Guid Guid { get; set; }
    }
}
