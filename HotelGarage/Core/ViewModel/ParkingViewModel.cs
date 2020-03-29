using HotelGarage.Core.Dto;
using HotelGarage.Core.Model;
using System.Collections.Generic;

namespace HotelGarage.Core.ViewModel
{
    public class ParkingViewModel
    {
        public IList<ParkingPlaceDto> ParkingPlaceDtos { get; set; }
        public IList<ReservationDto> TodaysReservations { get; set; }
        public IList<ReservationDto> NoShowReservations { get; set; }
        public IList<ReservationDto> InHouseReservations { get; set; }
        public IList<string> FreeParkingPlaceNames { get; set; }
        public OccupancyNumbers[] OccupancyNumbers { get; set; }

        public ParkingViewModel(IUnitOfWork unitOfWork, ParkingPlaceDto parkingPlaceDto)
        {
            ParkingPlaceDtos = parkingPlaceDto.GetParkingPlaceDtos(unitOfWork);
            TodaysReservations = ReservationDto.GetArrivingReservationDtos(unitOfWork);
            NoShowReservations = ReservationDto.GetNoShowReservationDtos(unitOfWork);
            InHouseReservations = ReservationDto.GetInhouseReservationDtos(unitOfWork);
            FreeParkingPlaceNames = unitOfWork.ParkingPlaces.GetFreeParkingPlaceNames();
            OccupancyNumbers = unitOfWork.Reservations.GetOccupancyNumbers(numberOfParkingPlaces: 19);
        }
    }
}