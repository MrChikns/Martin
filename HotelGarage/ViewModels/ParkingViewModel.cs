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
        public IList<ReservationDto> TodaysReservations { get; set; }
        public IList<ReservationDto> NoShowReservations { get; set; }
        public IList<ReservationDto> InHouseReservations { get; set; }
        public IList<string> FreeParkingPlaces { get; set; }
        public int ParkingPlaceName { get; set; }
        public int[] NumberOfFreeParkingPlacesArray { get; set; }

        public ParkingViewModel(IList<ParkingPlaceDto> parkingPlaceDtos,
            IList<ReservationDto> arrivingReservationDtos, IList<ReservationDto> noShowReservationDtos,
            IList<ReservationDto> inHouseReservationDtos, List<string> freePlacesList, int[] freePPlacesArray)
        {
            this.ParkingPlaceDtos = parkingPlaceDtos;
            this.TodaysReservations = arrivingReservationDtos;
            this.NoShowReservations = noShowReservationDtos;
            this.FreeParkingPlaces = freePlacesList;
            this.InHouseReservations = inHouseReservationDtos;
            this.NumberOfFreeParkingPlacesArray = freePPlacesArray;
        }
    }
}