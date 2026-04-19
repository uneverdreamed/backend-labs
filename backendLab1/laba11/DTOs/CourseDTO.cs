namespace laba11.DTOs
{
    public class CreateCourseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }
    }
}