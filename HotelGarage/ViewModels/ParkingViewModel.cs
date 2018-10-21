using HotelGarage.Dtos;
using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.ViewModels
{
    public class ParkingViewModel
    {
        public IList<ParkingPlaceDto> ParkingPlaceDtos { get; set; }
        public IList<Reservation> TodaysReservations { get; set; }
    }
}