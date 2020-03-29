using System;
using System.Collections.Generic;
using HotelGarage.Core.Model;

namespace HotelGarage.Core.Repository
{
    public interface IReservationRepository
    {
        List<Reservation> GetAllReservations();
        List<Reservation> GetInhouseReservations();
        List<Reservation> GetInhouseReservations(DateTime date);
        List<string> GetLicensePlates();
        List<Reservation> GetNoShowReservations();
        OccupancyNumbers[] GetOccupancyNumbers(int numberOfParkingPlaces);
        Reservation GetReservation(int reservationId, bool includeCar);
        List<Reservation> GetReturningReservations();
        List<Reservation> GetReservations(DateTime arrival, ReservationState state);
        void AddReservation(Reservation reservation);
    }
}