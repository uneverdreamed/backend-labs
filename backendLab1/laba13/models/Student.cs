using System.Text.Json.Serialization;

namespace laba12.models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public ICollection<Enroll> Enrolls { get; set; } = new List<Enroll>();
    }
}