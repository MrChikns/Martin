using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace HotelGarage.Dtos
{
    public class ParkingPlaceDto
    {
        public int Id { get; set; }
        public int? ReservationId { get; set; }
        public string LicensePlate { get; set; }
        public string Departure { get; set; }
        public string PPlaceName { get; set; }
        public string StateOfPlace { get; set; }

        public string DepartureBootbox { get; set; }
        public string ArrivalBootbox { get; internal set; }
        public object NameBootbox { get; internal set; }
        public object RoomBootbox { get; internal set; }
        public string CarTypeBootbox { get; internal set; }
        public int? PricePerNightBootbox { get; internal set; }
        public string LicensePlateBootbox { get; internal set; }
        public string EmployeeBootbox { get; internal set; }

        public string ParkPlaceShortLicensePlate { get; internal set; }
    }
}