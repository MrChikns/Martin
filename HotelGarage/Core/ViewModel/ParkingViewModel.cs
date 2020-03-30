using HotelGarage.Core.Dto;
using HotelGarage.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

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
            OccupancyNumbers = GetOccupancyNumbers(unitOfWork);
        }

        private OccupancyNumbers[] GetOccupancyNumbers(IUnitOfWork unitOfWork)
        {
            var totalDays = 7;
            var occupancyNumbers = new OccupancyNumbers[totalDays];
            var guestParkingPlaces = unitOfWork.ParkingPlaces.GetParkingPlaces(ParkingPlaceType.Garage, ParkingPlaceType.Outside);

            for (int dayNumber = 0; dayNumber < totalDays; dayNumber++)
            {
                var reservations = unitOfWork.Reservations.GetInhouseReservations(DateTime.Today.AddDays(dayNumber));

                occupancyNumbers[dayNumber].FreePlacesCount = guestParkingPlaces.Count - reservations.Count;
                occupancyNumbers[dayNumber].OccupiedByEmployeesCount = reservations.Where(r => r.Car.IsEmployee.Equals(true)).Count();
            }

            return occupancyNumbers;
        }
    }
}