using System.Text.Json.Serialization;

namespace laba12.models
{
    // связующая сущность: один студент — много курсов, один курс — много студентов
    public class Enroll
    {
        public int Id { get; set; }
        public int StudentId { get; set; }

        [JsonIgnore]
        public Student Student { get; set; } = null!;

        public int CourseId { get; set; }

        [JsonIgnore]
        public Course Course { get; set; } = null!;

        public DateTime EnrolledAt { get; set; }
        public string? Grade { get; set; }
    }
}