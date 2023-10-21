using Newtonsoft.Json.Linq;

namespace AssessmentAEM.Models
{
    public class DataViewModel
    {
        //public string PlatformName { get; set; }
        public int Id { get; set; }
        //public int PlatformId { get; set; }
        public string UniqueName { get; set; }


        public double Latitude { get; set; } 


        public double Longitude { get; set; } 
        //public DateTime lastUpdatengitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual List<Well> Wells { get; set; }
    }
}
public class TokenResponse
{
    public string Token { get; set; }
}
public class EndpointRequest
{
    public string Endpoint { get; set; }
}