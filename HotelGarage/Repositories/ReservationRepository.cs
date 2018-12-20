using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HotelGarage.Repositories
{
    public class ReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Reservation GetReservation(int reservationId)
        {
            return _context.Reservations.FirstOrDefault(r => r.Id == reservationId);
        }
        
        public Reservation GetReservationCar(int reservationId)
        {
            return _context.Reservations.Include(c => c.Car).First(r => r.Id == reservationId);
        }

        public List<Reservation> GetTodaysReservationsCar()
        {
            return _context.Reservations
                .Where(a => (DbFunctions.TruncateTime(a.Arrival) == DateTime.Today.Date
                         && a.StateOfReservationId == StateOfReservation.Reserved)
                    ||(a.StateOfReservationId == StateOfReservation.TemporaryLeave))
                    .Include(c => c.Car)
                .ToList();
        }

        public List<Reservation> GetNoShowReservationsCar()
        {
            return _context.Reservations
                .Where(a => DbFunctions.TruncateTime(a.Arrival) < DateTime.Today.Date
                    && a.StateOfReservationId == StateOfReservation.Reserved)
                    .Include(c => c.Car)
                .ToList();
        }
        
        public List<Reservation> GetInhouseReservationsCar()
        {
            return _context.Reservations
                .Where(a => a.StateOfReservationId == StateOfReservation.Inhouse)
                .Include(c => c.Car)
                .ToList();
        }

        public List<Reservation> GetAllReservationsCar()
        {
            return _context.Reservations
                .Include(c => c.Car)
                .ToList();
        }

        public string GetStateOfReservationName(int id)
        {
            return _context.StateOfReservations.First(s => s.Id == id).State;
        }

        public List<string> GetLicensePlates()
        {
            var list = new List<string>();

            foreach (var res in _context.Reservations)
            {
                list.Add(res.LicensePlate);
            }
                       
            return list;
        }

        public List<Reservation> GetReturningReservationsCars()
        {
            return _context.Reservations
                .Where(c => c.Car.NumberOfStays >= 2)
                .Include(c => c.Car)
                .ToList();
        }
        public int GetNumberOfFreeParkingPlaces(DateTime date)
        {
            int totalNumberOfParkingPlaces = 19;

            int numberOfOccupiedParkingPlaces = _context.Reservations
                .Where(r =>
                    (r.StateOfReservationId == StateOfReservation.Reserved
                        || r.StateOfReservationId == StateOfReservation.Inhouse
                        || r.StateOfReservationId == StateOfReservation.TemporaryLeave)
                    && (r.Arrival.Year <= date.Year
                        && r.Arrival.Month <= date.Month
                        && r.Arrival.Day <= date.Day
                        && r.Departure.Year >= date.Year
                        && r.Departure.Month >= date.Month
                        && r.Departure.Day > date.Day))
                .Count();

            return totalNumberOfParkingPlaces - numberOfOccupiedParkingPlaces;
        }

        public int[] GetNumbersOfFreeParkingPlacesArray()
        {
            int[] freeplaces = new int[7];

            for (int i = 0; i < 7; i++)
            {
                freeplaces[i] = this.GetNumberOfFreeParkingPlaces(DateTime.Today.AddDays(i));
            }

            return freeplaces;
        }
    }
}