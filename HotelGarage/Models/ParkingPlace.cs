using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.Models
{
    public class ParkingPlace
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Reservation Reservation { get; set; }

        public int StateOfPlaceId { get; set; }

        public StateOfPlace StateOfPlace {get;set;}
    }
}