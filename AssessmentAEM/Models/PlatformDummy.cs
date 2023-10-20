using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AssessmentAEM.Models
{
    public class PlatformDummy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string UniqueName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        //public DateTime LastUpdate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<WellDummy> Well { get; set; }
    }
}
