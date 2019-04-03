using HotelGarage.Core.Models;
using HotelGarage.Persistence;
using HotelGarage.Persistence.Repositories;
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
        public byte StateOfReservationId { get; set; }
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
            StateOfReservationId = reservation.StateOfReservationId;
            IsRegistered = reservation.IsRegistered;
        }

        public static IList<ReservationDto> GetArrivingReservations(IUnitOfWork unitOfWork)
        {
            var arrivingResDtos = new List<ReservationDto>();
            foreach (var reservation in unitOfWork.Reservations.GetTodaysReservationsCar())
            {
                string parkingPlaceName;

                if (reservation.ParkingPlaceId != 0)
                    parkingPlaceName = unitOfWork.ParkingPlaces.GetParkingPlace(reservation).Name;
                else
                    parkingPlaceName = "Nepřiřazeno";

                arrivingResDtos.Add(new ReservationDto(reservation, parkingPlaceName));
            }
            return arrivingResDtos.OrderBy(o => o.ParkingPlaceId).ToList();
        }

        public static IList<ReservationDto> GetNoShowReservations(IUnitOfWork unitOfWork)
        {
            var nSResDtos = new List<ReservationDto>();
            foreach (var reservation in unitOfWork.Reservations.GetNoShowReservationsCar())
            {
                //asi smazat
                string parkingPlaceName;

                if (reservation.ParkingPlaceId != 0)
                    parkingPlaceName = unitOfWork.ParkingPlaces.GetParkingPlace(reservation).Name;
                else
                    parkingPlaceName = "Nepřiřazeno";

                nSResDtos.Add(new ReservationDto(reservation, parkingPlaceName));
            }
            return nSResDtos.OrderBy(o => o.Arrival).ToList();
        }
        
        public static IList<ReservationDto> GetInhouseReservations(IUnitOfWork unitOfWork)
        {
            var inhouseResDtos = new List<ReservationDto>();
            foreach (var reservation in unitOfWork.Reservations.GetInhouseReservationsCar())
            {
                inhouseResDtos.Add(new ReservationDto(reservation, 
                    unitOfWork.ParkingPlaces.GetParkingPlace(reservation).Name));
            }

            return inhouseResDtos.OrderBy(o => o.ParkingPlaceId).ToList();
        }
    }
}