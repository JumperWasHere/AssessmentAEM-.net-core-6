﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
namespace AssessmentAEM.Models
{
    public class Platform
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
