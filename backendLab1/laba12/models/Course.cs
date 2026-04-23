using System.Text.Json.Serialization;

namespace laba12.models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }

        [JsonIgnore]
        public ICollection<Enroll> Enrolls { get; set; } = new List<Enroll>();
    }
}