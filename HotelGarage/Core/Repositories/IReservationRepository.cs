using System;
using System.Collections.Generic;
using HotelGarage.Core.Models;

namespace HotelGarage.Core.Repositories
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
        List<Reservation> GetTodaysReservations();
        void AddReservation(Reservation reservation);
    }
}