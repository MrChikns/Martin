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
        public IList<ParkingPlaceDto> ParkingPlaceDtos { get; set; }

        public IList<Reservation> TodaysReservations { get; set; }
        public IEnumerable<string> FreeParkingPlaces { get; set; }
        public int ReservationId { get; set; }
        public int ParkingPlaceId { get; set; }
    }
}