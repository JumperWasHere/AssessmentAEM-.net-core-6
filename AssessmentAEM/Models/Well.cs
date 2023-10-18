namespace AssessmentAEM.Models
{
    public class Well
    {
        public int id { get; set; }
        public int platformId { get; set; }
        public string uniqueName { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

    }
}
