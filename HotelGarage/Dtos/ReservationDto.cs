using HotelGarage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }

        public string CarLicensePlate { get; set; }
        public string CarGuestsName { get; set; }
        public string Arrival { get; set; }
        public byte StateOfReservationId { get; set; }
        public bool IsRegistered { get; set; }

        public int ParkingPlaceId { get; set; }
        public string ParkingPlaceName { get; set; }

        public ReservationDto(int id, string carLicensePlate, string carGuestsName, int parkingPlaceId, 
            string parkingPlaceName, string arrival, byte stateOfRes, bool isRegistered)
        {
            Id = id;
            CarLicensePlate = carLicensePlate;
            CarGuestsName = carGuestsName;
            ParkingPlaceId = parkingPlaceId;
            ParkingPlaceName = parkingPlaceName;
            Arrival = arrival;
            StateOfReservationId = stateOfRes;
            IsRegistered = isRegistered;
        }

        public static IList<ReservationDto> GetArrivingReservations(ReservationRepository reservationRepository, 
            ParkingPlaceRepository parkingPlaceRepository)
        {
            var arrivingResDtos = new List<ReservationDto>();
            foreach (var reservation in reservationRepository.GetTodaysReservationsCar())
            {
                string parkingPlaceName;

                if (reservation.ParkingPlaceId != 0)
                    parkingPlaceName = parkingPlaceRepository.GetParkingPlace(reservation).Name;
                else
                    parkingPlaceName = "Nepřiřazeno";

                arrivingResDtos.Add(new ReservationDto(reservation.Id, reservation.Car.LicensePlate,
                    reservation.Car.GuestsName, reservation.ParkingPlaceId, parkingPlaceName,
                    reservation.Arrival.ToShortDateString(),reservation.StateOfReservationId,reservation.IsRegistered));
            }
            return arrivingResDtos;
        }

        public static IList<ReservationDto> GetNoShowReservations(ReservationRepository reservationRepository,
            ParkingPlaceRepository parkingPlaceRepository)
        {
            var nSResDtos = new List<ReservationDto>();
            foreach (var reservation in reservationRepository.GetNoShowReservationsCar())
            {
                //asi smazat
                string parkingPlaceName;

                if (reservation.ParkingPlaceId != 0)
                    parkingPlaceName = parkingPlaceRepository.GetParkingPlace(reservation).Name;
                else
                    parkingPlaceName = "Nepřiřazeno";

                nSResDtos.Add(new ReservationDto(reservation.Id, reservation.Car.LicensePlate,
                    reservation.Car.GuestsName, reservation.ParkingPlaceId, parkingPlaceName,
                    reservation.Arrival.ToShortDateString(),reservation.StateOfReservationId,reservation.IsRegistered));
            }
            return nSResDtos;
        }
        
        public static IList<ReservationDto> GetInhouseReservations(ReservationRepository reservationRepository,
            ParkingPlaceRepository parkingPlaceRepository)
        {
            var inhouseResDtos = new List<ReservationDto>();
            foreach (var reservation in reservationRepository.GetInhouseReservationsCar())
            {
                inhouseResDtos.Add(new ReservationDto(reservation.Id, reservation.Car.LicensePlate,
                    reservation.Car.GuestsName, reservation.ParkingPlaceId, 
                    parkingPlaceRepository.GetParkingPlace(reservation).Name,
                    reservation.Arrival.ToShortDateString(), reservation.StateOfReservationId,reservation.IsRegistered));
            }
            return inhouseResDtos;
        }
    }
}