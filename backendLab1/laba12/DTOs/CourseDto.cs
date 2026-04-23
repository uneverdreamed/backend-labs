using System.ComponentModel.DataAnnotations;

namespace laba12.DTOs
{
    public class CreateCourseDto
    {
        [Required(ErrorMessage = "Название курса обязательно")]
        [MinLength(3, ErrorMessage = "Название должно содержать минимум 3 символа")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описание курса обязательно")]
        public string Description { get; set; } = string.Empty;

        [Range(1, 10, ErrorMessage = "Количество кредитов должно быть от 1 до 10")]
        public int Credits { get; set; }
    }
}