namespace laba9.settings
{
    public class AppSettings
    {
        public const string SectionName = "AppSettings";

        public string ApplicationName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public int MaxItemsPerPage { get; set; }
        public bool EnableLogging { get; set; }
    }
    public class DatabaseSettings
    {
        public const string SectionName = "DatabaseSettings";

        public string ConnectionString { get; set; } = string.Empty;
        public int Timeout { get; set; }
    }
}