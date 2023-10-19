namespace AssessmentAEM.Models
{
    public class PlatformDummy
    {
        public int id { get; set; }
        public string uniqueName { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public DateTime lastUpdate { get; set; }
        public List<WellDummy> Well { get; set; }
    }
}
