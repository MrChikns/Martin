using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }

        public string CarLicensePlate { get; set; }
        public string CarGuestsName { get; set; }
        public string Arrival { get; set; }

        public int ParkingPlaceId { get; set; }
        public string ParkingPlaceName { get; set; }

        public ReservationDto(int id, string carLicensePlate,
           string carGuestsName, int parkingPlaceId, string parkingPlaceName, string arrival)
        {
            Id = id;
            CarLicensePlate = carLicensePlate;
            CarGuestsName = carGuestsName;
            ParkingPlaceId = parkingPlaceId;
            ParkingPlaceName = parkingPlaceName;
            Arrival = arrival;
        }
    }
}