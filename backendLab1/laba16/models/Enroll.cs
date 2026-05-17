using System.Text.Json.Serialization;

namespace laba11.Models
{
    // один студент может быть записан на много курсов, один курс иметь много студентов
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
        public string? Grade { get; set; } // оценка, может быть null если курс ещё не завершён
    }
}