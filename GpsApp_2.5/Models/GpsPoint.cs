using System;
using System.ComponentModel.DataAnnotations;

namespace GpsApp_2._5.Models
{
    public class GpsPoint
    {
        public int Id { get; set; }
        //[Required]
        public string PlaceName { get; set; }
        //[Required]
        public DateTime DateTime { get; set; }
        //[Required]
        public string Latitude { get; set; }
        //[Required]
        public string Longitude { get; set; }
        //[Required]
        public string Note { get; set; }
        [Required]
        public int RouteId { get; set; }

        public virtual Route Route { get; set; }
    }
}