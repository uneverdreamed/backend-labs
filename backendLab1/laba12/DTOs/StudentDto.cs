using System.ComponentModel.DataAnnotations;

namespace laba12.DTOs
{
    // DTO для создания/обновления студента — с атрибутами валидации для ModelState
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "Имя студента обязательно")]
        [MinLength(2, ErrorMessage = "Имя должно содержать минимум 2 символа")]
        [MaxLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Группа обязательна")]
        [RegularExpression(@"^\d{3}-\d{3}$", ErrorMessage = "Группа должна быть в формате XXX-XXX (например, 241-334)")]
        public string Group { get; set; } = string.Empty;
    }
}