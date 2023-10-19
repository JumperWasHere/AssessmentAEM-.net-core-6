namespace AssessmentAEM.Models
{
    public class Well
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }//FK
        public string UniqueName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        //public virtual Platform Platform { get; set; }//Navigation property

    }
}
