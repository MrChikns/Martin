using HotelGarage.Models;
using HotelGarage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.Dtos
{
    public class ReservationListDto
    {

        public string GuestsName { get; set; }
        public string Arrival { get; set; }
        public string Departure { get; set; }
        public string GuestRoomNumber { get; set; }
        public string TotalPrice { get; set; }
        public string ReservationState { get; set; }
        public string LicensePlate { get; set; }
        public string CarModel { get; set; }
        public string ParkingPlaceName { get; set; }
        public string IsEmployee { get; set; }
        public string NumberOfStays { get; set; }

        public ReservationListDto(Reservation reservation, ReservationRepository reservationRepository,
            ParkingPlaceRepository parkingPlaceRepository)
        {
            var nevyplneno = "Nevyplněno";

            GuestsName = (reservation.Car.GuestsName == null) ? nevyplneno : reservation.Car.GuestsName;
            Arrival = reservation.Arrival.ToString("yyyy.MM.dd hh:mm");
            Departure = reservation.Departure.ToString("yyyy.MM.dd hh:mm");
            GuestRoomNumber = (reservation.Car.GuestRoomNumber == null) ? 
                nevyplneno : reservation.Car.GuestRoomNumber.ToString();
            TotalPrice = reservation.Car.CalculateTotalPrice(reservation.Arrival, 
                reservation.Departure, reservation.Car.PricePerNight);
            ReservationState = reservationRepository.GetStateOfReservationName(reservation.StateOfReservationId);
            LicensePlate = reservation.LicensePlate;
            CarModel = (reservation.Car.CarModel == null) ? nevyplneno : reservation.Car.CarModel;
            ParkingPlaceName = (reservation.ParkingPlaceId == 0) ? nevyplneno : parkingPlaceRepository.GetParkingPlaceName(reservation.ParkingPlaceId);
            IsEmployee = (reservation.Car.IsEmployee) ? "Zaměstnanec" : "Host";
            NumberOfStays = reservation.Car.NumberOfStays.ToString();
        }

        public static IList<ReservationListDto> GetAllReservationDtos(ReservationRepository reservationRepository,
            ParkingPlaceRepository parkingPlaceRepository)
        {
            var allResListDto = new List<ReservationListDto>();

            foreach (var res in reservationRepository.GetAllReservationsCar())
            {
                allResListDto.Add(new ReservationListDto(res,reservationRepository,parkingPlaceRepository));
            }
            return allResListDto;
        }

        
    }
}