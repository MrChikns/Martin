using HotelGarage.Core.Models;
using HotelGarage.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace HotelGarage.Core.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public string CarLicensePlate { get; set; }
        public string CarGuestsName { get; set; }
        public string Arrival { get; set; }
        public string Departure { get; set; }
        public ReservationState State { get; set; }
        public bool IsRegistered { get; set; }
        public int ParkingPlaceId { get; set; }
        public string ParkingPlaceName { get; set; }

        private ReservationDto(Reservation reservation, string parkingPlaceName)
        {
            Id = reservation.Id;
            CarLicensePlate = reservation.LicensePlate;
            CarGuestsName = reservation.Car.GuestsName;
            ParkingPlaceId = reservation.ParkingPlaceId;
            ParkingPlaceName = parkingPlaceName;
            Arrival = reservation.Arrival.ToShortDateString();
            Departure = reservation.Departure.ToShortDateString();
            State = reservation.State;
            IsRegistered = reservation.IsRegistered;
        }

        public static IList<ReservationDto> GetArrivingReservationDtos(IUnitOfWork unitOfWork)
        {
            var todayReservations = unitOfWork.Reservations.GetReservations(arrival: System.DateTime.Today, state: ReservationState.Reserved);
            var arrivingReservationDtos = GetReservationDtos(unitOfWork, todayReservations);
            
            return arrivingReservationDtos.OrderBy(o => o.ParkingPlaceId).ToList();
        }

        public static IList<ReservationDto> GetNoShowReservationDtos(IUnitOfWork unitOfWork)
        {
            var noShowReservations = unitOfWork.Reservations.GetNoShowReservations();
            var noShowReservationDtos = GetReservationDtos(unitOfWork, noShowReservations);

            return noShowReservationDtos.OrderBy(o => o.Arrival).ToList();
        }
        
        public static IList<ReservationDto> GetInhouseReservationDtos(IUnitOfWork unitOfWork)
        {
            var inHouseReservations = unitOfWork.Reservations.GetInhouseReservations();
            var inhouseReservationDtos = GetReservationDtos(unitOfWork, inHouseReservations);

            return inhouseReservationDtos.OrderBy(o => o.ParkingPlaceId).ToList();
        }

        private static List<ReservationDto> GetReservationDtos(IUnitOfWork unitOfWork, List<Reservation> reservations)
        {
            var reservationDtos = new List<ReservationDto>();

            foreach (var reservation in reservations)
            {
                var parkingPlaceName = GetParkingPlaceName(unitOfWork, reservation.ParkingPlaceId);
                reservationDtos.Add(new ReservationDto(reservation, parkingPlaceName));
            }

            return reservationDtos;
        }

        private static string GetParkingPlaceName(IUnitOfWork unitOfWork, int parkingPlaceId)
        {
            if (parkingPlaceId != 0)
            {
                var parkingPlace = unitOfWork.ParkingPlaces.GetParkingPlace(parkingPlaceId, includeCarAndReservation: false);

                return parkingPlace.Name;
            }
            else
            {
                return Labels.NotAssigned;
            }
        }
    }
}