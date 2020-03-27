using HotelGarage.Core.Models;
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

        public ReservationDto(Reservation reservation, string parkingPlaceName)
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

        public static IList<ReservationDto> GetArrivingReservations(IUnitOfWork unitOfWork)
        {
            var arrivingResDtos = new List<ReservationDto>();
            foreach (var reservation in unitOfWork.Reservations.GetTodaysReservations())
            {
                string parkingPlaceName;

                if (reservation.ParkingPlaceId != 0)
                {
                    parkingPlaceName = unitOfWork.ParkingPlaces.GetParkingPlace(reservation).Name;
                }
                else
                {
                    parkingPlaceName = "Nepřiřazeno";
                }

                arrivingResDtos.Add(new ReservationDto(reservation, parkingPlaceName));
            }
            return arrivingResDtos.OrderBy(o => o.ParkingPlaceId).ToList();
        }

        public static IList<ReservationDto> GetNoShowReservations(IUnitOfWork unitOfWork)
        {
            var noShowReservationDtos = new List<ReservationDto>();
            foreach (var reservation in unitOfWork.Reservations.GetNoShowReservations())
            {
                string parkingPlaceName;

                if (reservation.ParkingPlaceId != 0)
                {
                    parkingPlaceName = unitOfWork.ParkingPlaces.GetParkingPlace(reservation).Name;
                }
                else
                {
                    parkingPlaceName = "Nepřiřazeno";
                }

                noShowReservationDtos.Add(new ReservationDto(reservation, parkingPlaceName));
            }
            return noShowReservationDtos.OrderBy(o => o.Arrival).ToList();
        }
        
        public static IList<ReservationDto> GetInhouseReservations(IUnitOfWork unitOfWork)
        {
            var inhouseReservationDtos = new List<ReservationDto>();
            foreach (var reservation in unitOfWork.Reservations.GetInhouseReservations())
            {
                var parkingPlaceName = unitOfWork.ParkingPlaces.GetParkingPlace(reservation).Name;
                inhouseReservationDtos.Add(new ReservationDto(reservation, parkingPlaceName));
            }

            return inhouseReservationDtos.OrderBy(o => o.ParkingPlaceId).ToList();
        }
    }
}