using System;
using System.Collections.Generic;
using HotelGarage.Core.Models;

namespace HotelGarage.Core.Repositories
{
    public interface IReservationRepository
    {
        List<Reservation> GetAllReservationsCar();
        List<Reservation> GetInhouseReservationsCar();
        List<Reservation> GetInhouseReservationsFromSelectedDay(DateTime date);
        List<string> GetLicensePlates();
        List<Reservation> GetNoShowReservationsCar();
        OccupancyNumbersOfTheDay[] GetNumberOfFreeParkingPlacesAndPlacesOccupiedByEmployeesArray();
        Reservation GetReservation(int reservationId);
        Reservation GetReservationCar(int reservationId);
        List<Reservation> GetReturningReservationsCars();
        string GetStateOfReservationName(int id);
        List<Reservation> GetTodaysReservationsCar();
        void AddReservation(Reservation reservation);
    }
}