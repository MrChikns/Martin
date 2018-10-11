using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelGarage.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public string LicensePlate { get; set; }

        public Car Car { get; set; }

        [Required]
        public DateTime Arrival { get; set; }

        [Required]
        public DateTime Departure { get; set; }

        [Required]
        public bool IsRegistered{ get; set; }


    }
}