using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GpsApp_2._5.Models
{
    public class Route
    {
        [Required]
        public int RouteId { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime LastModified { get; set; }

        public virtual ICollection<GpsPoint> PointsList { get; set; }
    }
}