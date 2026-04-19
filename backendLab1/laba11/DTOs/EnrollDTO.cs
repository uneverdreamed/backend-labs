namespace laba11.DTOs
{
    public class CreateEnrollDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string? Grade { get; set; }
    }

    public class UpdateEnrollDto
    {
        public string? Grade { get; set; }
    }
}