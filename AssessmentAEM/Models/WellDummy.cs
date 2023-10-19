namespace AssessmentAEM.Models
{
    public class WellDummy
    {
        public int id { get; set; }
        public int platformId { get; set; }
        public string uniqueName { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public DateTime lastUpdate { get; set; }
        //public DateTime updatedAt { get; set; }
    }
}
