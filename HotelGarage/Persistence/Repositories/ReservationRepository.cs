using HotelGarage.Core.Model;
using HotelGarage.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HotelGarage.Persistence.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public Reservation GetReservation(int id, bool includeCar)
        {
            if (includeCar)
            {
                return _context.Reservations.Include(c => c.Car).FirstOrDefault(r => r.Id == id);
            }

            return _context.Reservations.FirstOrDefault(r => r.Id == id);
        }

        public List<Reservation> GetReservations(DateTime arrivalDateTime, ReservationState state)
        {
            return _context.Reservations
                .Where(a =>
                    (DbFunctions.TruncateTime(a.Arrival) == arrivalDateTime.Date && a.State == state) ||
                    a.State == ReservationState.TemporaryLeave
                )
                .Include(c => c.Car)
                .ToList();
        }
               
        public List<Reservation> GetNoShowReservations()
        {
            return _context.Reservations
                .Where(a => DbFunctions.TruncateTime(a.Arrival) < DateTime.Today.Date && a.State == ReservationState.Reserved)
                .Include(c => c.Car)
                .ToList();
        }
        
        public List<Reservation> GetInhouseReservations()
        {
            return _context.Reservations
                .Where(a => a.State == ReservationState.Inhouse)
                .Include(c => c.Car)
                .ToList();
        }

        public List<Reservation> GetAllReservations()
        {
            return _context.Reservations
                .Include(c => c.Car)
                .ToList();
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
        
        public List<Reservation> GetInhouseReservations(DateTime inhouseDate)
        {
            return _context.Reservations
                .Include(r => r.Car)
                .Where(r =>
                    (r.State == ReservationState.Reserved || r.State == ReservationState.Inhouse || r.State == ReservationState.TemporaryLeave)
                    && (DbFunctions.TruncateTime(r.Arrival) <= inhouseDate
                    && DbFunctions.TruncateTime(r.Departure) > inhouseDate)
                )
                .ToList();
        }

        public void AddReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
        }
    }
}