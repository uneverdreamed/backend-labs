using System.Text.Json.Serialization;

namespace laba11.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }

        // навигационное свойство (один курс может иметь много записей студентов)
        [JsonIgnore]
        public ICollection<Enroll> Enrolls { get; set; } = new List<Enroll>();
    }
}