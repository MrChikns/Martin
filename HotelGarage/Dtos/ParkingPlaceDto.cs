using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.Dtos
{
    public class ParkingPlaceDto
    {
        public int Id { get; set; }
        public int? ReservationId { get; set; }
        public string LicensePlate { get; set; }
        public string Departure { get; set; }
        public string Name { get; set; }
        public string StateOfPlace { get; set; }
    }
}