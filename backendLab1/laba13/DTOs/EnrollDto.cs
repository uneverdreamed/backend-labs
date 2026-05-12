using System.ComponentModel.DataAnnotations;

namespace laba12.DTOs
{
    public class CreateEnrollDto
    {
        [Required(ErrorMessage = "StudentId обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "StudentId должен быть положительным числом")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "CourseId обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "CourseId должен быть положительным числом")]
        public int CourseId { get; set; }

        public string? Grade { get; set; }
    }

    public class UpdateEnrollDto
    {
        [RegularExpression(@"^[2-5]$", ErrorMessage = "Оценка должна быть от 2 до 5")]
        public string? Grade { get; set; }
    }
}