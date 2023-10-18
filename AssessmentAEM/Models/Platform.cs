namespace AssessmentAEM.Models
{
    public class Platform
    {
        public string PlatformName { get; set; }
        public int id { get; set; }
        public int PlatformId { get; set; }
        public string UniqueName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
