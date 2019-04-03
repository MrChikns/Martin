using HotelGarage.Core.Models;
using HotelGarage.Persistence;
using HotelGarage.Persistence.Repositories;
using System.Collections.Generic;
using HotelGarage.Core.Dtos;

namespace HotelGarage.Core.ViewModels
{
    public class ParkingViewModel
    {
        public IList<ParkingPlaceDto> ParkingPlaceDtos { get; set; }
        public IList<ReservationDto> TodaysReservations { get; set; }
        public IList<ReservationDto> NoShowReservations { get; set; }
        public IList<ReservationDto> InHouseReservations { get; set; }
        public IList<string> FreeParkingPlaces { get; set; }
        public int ParkingPlaceName { get; set; }
        public OccupancyNumbersOfTheDay[] NumberOfFreeAndEmployeeOccupiedParkingPlacesArray { get; set; }

        public ParkingViewModel(IUnitOfWork unitOfWork)
        {
            this.ParkingPlaceDtos = ParkingPlaceDto.GetParkingPlaceDtos(unitOfWork);
            this.TodaysReservations = ReservationDto.GetArrivingReservations(unitOfWork);
            this.NoShowReservations = ReservationDto.GetNoShowReservations(unitOfWork);
            this.InHouseReservations = ReservationDto.GetInhouseReservations(unitOfWork);
            this.FreeParkingPlaces = unitOfWork.ParkingPlaces.GetNamesOfFreeParkingPlaces();
            this.NumberOfFreeAndEmployeeOccupiedParkingPlacesArray = unitOfWork.Reservations.GetNumberOfFreeParkingPlacesAndPlacesOccupiedByEmployeesArray();
        }
    }
}