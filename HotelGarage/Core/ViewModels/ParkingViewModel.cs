using HotelGarage.Core.Dtos;
using HotelGarage.Core.Models;
using System.Collections.Generic;

namespace HotelGarage.Core.ViewModels
{
    public class ParkingViewModel
    {
        public IList<ParkingPlaceDto> ParkingPlaceDtos { get; set; }
        public IList<ReservationDto> TodaysReservations { get; set; }
        public IList<ReservationDto> NoShowReservations { get; set; }
        public IList<ReservationDto> InHouseReservations { get; set; }
        public IList<string> FreeParkingPlaces { get; set; }
        public OccupancyNumbersOfTheDay[] NumberOfFreeAndEmployeeOccupiedParkingPlacesArray { get; set; }

        public ParkingViewModel(IUnitOfWork unitOfWork)
        {
            ParkingPlaceDtos = ParkingPlaceDto.GetParkingPlaceDtos(unitOfWork);
            TodaysReservations = ReservationDto.GetArrivingReservations(unitOfWork);
            NoShowReservations = ReservationDto.GetNoShowReservations(unitOfWork);
            InHouseReservations = ReservationDto.GetInhouseReservations(unitOfWork);
            FreeParkingPlaces = unitOfWork.ParkingPlaces.GetNamesOfFreeParkingPlaces();
            NumberOfFreeAndEmployeeOccupiedParkingPlacesArray = unitOfWork.Reservations.GetNumberOfFreeParkingPlacesAndPlacesOccupiedByEmployeesArray();
        }
    }
}