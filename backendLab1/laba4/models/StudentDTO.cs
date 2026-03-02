using System.ComponentModel.DataAnnotations;

namespace laba4.models
{
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 100 символов")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Группа обязательна")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Название группы должно быть от 2 до 20 символов")]
        public string Group { get; set; } = string.Empty;
    }
}
