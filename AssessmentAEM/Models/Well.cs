using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AssessmentAEM.Models
{
    public class Well
    {
        //[Key]
        //[Column(Order = 0)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int PlatformId { get; set; }//FK
        public string UniqueName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
