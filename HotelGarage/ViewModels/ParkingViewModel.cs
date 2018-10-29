using HotelGarage.Dtos;
using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HotelGarage.ViewModels
{
    public class ParkingViewModel
    {
        // parking places
        public IList<ParkingPlaceDto> ParkingPlaceDtos { get; set; }

        // arriving reservations
        public IList<ArrivingReservationDto> TodaysReservations { get; set; }
        public IList<string> FreeParkingPlaces { get; set; }
        public int ParkingPlaceName { get; set; }
    }
}