using HotelGarage.Dtos;
using HotelGarage.Models;
using HotelGarage.Repositories;
using System.Collections.Generic;

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
        public OccupancyNumbersOfTheDay[] NumberOfFreeAndEmployeeOccupiedParkingPlacesArray { get; set; }

        public ParkingViewModel(ParkingPlaceRepository parkingPlaceRepository, 
                StateOfPlaceRepository stateOfPlaceRepository, 
                ReservationRepository reservationRepository,
                CarRepository carRepository,
                ApplicationDbContext context)
        {
            this.ParkingPlaceDtos = ParkingPlaceDto.GetParkingPlaceDtos(parkingPlaceRepository, stateOfPlaceRepository, carRepository, context);
            this.TodaysReservations = ReservationDto.GetArrivingReservations(reservationRepository, parkingPlaceRepository);
            this.NoShowReservations = ReservationDto.GetNoShowReservations(reservationRepository, parkingPlaceRepository);
            this.InHouseReservations = ReservationDto.GetInhouseReservations(reservationRepository, parkingPlaceRepository);
            this.FreeParkingPlaces = parkingPlaceRepository.GetNamesOfFreeParkingPlaces();
            this.NumberOfFreeAndEmployeeOccupiedParkingPlacesArray = reservationRepository.GetNumberOfFreeParkingPlacesAndPlacesOccupiedByEmployeesArray();
        }
    }
}