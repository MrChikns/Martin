using HotelGarage.Models;
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
        public string NameBootbox { get; internal set; }
        public int? RoomBootbox { get; internal set; }
        public string CarTypeBootbox { get; internal set; }
        public int? PricePerNightBootbox { get; internal set; }
        public string LicensePlateBootbox { get; internal set; }
        public string EmployeeBootbox { get; internal set; }

        public string ParkPlaceShortLicensePlate { get; internal set; }

        public ParkingPlaceDto(int parkingPlaceId, int? resId, string licensePlate,
            string departure, string pPlaceName, string stateOfPlaceName, string arrival,
            string pPlaceGuestsName, int? pPRoom, string pPlaceCar, int? pPPrice, string isEmployee)
        {
            Id = parkingPlaceId;
            ReservationId = resId;
            LicensePlate = licensePlate;
            Departure = departure;
            PPlaceName = pPlaceName;
            StateOfPlace = stateOfPlaceName;

            DepartureBootbox = departure.Replace(" ", "_");
            ArrivalBootbox = arrival.Replace(" ", "_");
            NameBootbox = pPlaceGuestsName.Replace(" ", "_");
            RoomBootbox = pPRoom;
            CarTypeBootbox = pPlaceCar.Replace(" ", "_");
            PricePerNightBootbox = pPPrice;
            LicensePlateBootbox = licensePlate.Replace(" ", "_");
            EmployeeBootbox = isEmployee;
            ParkPlaceShortLicensePlate = (licensePlate.Length > 20) ? licensePlate.Substring(0, 20) : licensePlate;
        }
    }
}