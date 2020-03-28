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
        public IList<string> FreeParkingPlaceNames { get; set; }
        public OccupancyNumbersOfTheDay[] NumberOfFreeAndEmployeeOccupiedParkingPlacesArray { get; set; }

        public ParkingViewModel(IUnitOfWork unitOfWork)
        {
            var parkingPlaceDto = new ParkingPlaceDto();
            ParkingPlaceDtos = parkingPlaceDto.GetParkingPlaceDtos(unitOfWork);
            TodaysReservations = ReservationDto.GetArrivingReservations(unitOfWork);
            NoShowReservations = ReservationDto.GetNoShowReservations(unitOfWork);
            InHouseReservations = ReservationDto.GetInhouseReservations(unitOfWork);
            FreeParkingPlaceNames = unitOfWork.ParkingPlaces.GetFreeParkingPlaceNames();
            NumberOfFreeAndEmployeeOccupiedParkingPlacesArray = unitOfWork.Reservations.GetFreeandEmployeeParkingPlacesCount();
        }
    }
}