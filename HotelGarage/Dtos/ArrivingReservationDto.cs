using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.Dtos
{
    public class ArrivingReservationDto
    {
        public int Id { get; set; }

        public string CarLicensePlate { get; set; }
        public string CarGuestsName { get; set; }

        public int ParkingPlaceId { get; set; }
        public string ParkingPlaceName { get; set; }

        public ArrivingReservationDto(int id, string carLicensePlate,
           string carGuestsName, int parkingPlaceId, string parkingPlaceName)
        {
            Id = id;
            CarLicensePlate = carLicensePlate;
            CarGuestsName = carGuestsName;
            ParkingPlaceId = parkingPlaceId;
            ParkingPlaceName = parkingPlaceName;
        }
    }
}