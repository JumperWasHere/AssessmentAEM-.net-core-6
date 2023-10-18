﻿namespace AssessmentAEM.Models
{
    public class Platform
    {
        //public string PlatformName { get; set; }
        public int id { get; set; }
        //public int PlatformId { get; set; }
        public string uniqueName { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        //public DateTime lastUpdatengitude { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public List<Well> Well { get; set; }

    }
}
