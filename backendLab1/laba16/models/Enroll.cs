using System.Text.Json.Serialization;

namespace laba16.Models;

// Один студент может быть записан на много курсов, один курс иметь много студентов
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

    // Оценка, может быть null если курс ещё не завершён
    public string? Grade { get; set; }
}