using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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

        public List<Reservation> GetReturningReservationsCars()
        {
            return _context.Reservations
                .Where(c => c.Car.NumberOfStays >= 2)
                .Include(c => c.Car)
                .ToList();
        }
    }
}