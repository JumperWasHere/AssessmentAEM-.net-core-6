namespace AssessmentAEM.Models
{
    public class PlatformDummy
    {
        public int Id { get; set; }
        public string UniqueName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<WellDummy> Wells { get; set; }
    }
}
