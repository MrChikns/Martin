﻿using HotelGarage.Core.Models;
using HotelGarage.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HotelGarage.Persistence.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public Reservation GetReservation(int id)
        {
            return _context.Reservations.Include(c => c.Car).First(r => r.Id == id);
        }

        public List<Reservation> GetTodaysReservations()
        {
            return _context.Reservations
                .Where(a => (
                    DbFunctions.TruncateTime(a.Arrival) == DateTime.Today.Date && a.State == ReservationState.Reserved)
                    || (a.State == ReservationState.TemporaryLeave)
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

        public List<Reservation> GetReturningReservations()
        {
            return _context.Reservations
                .Where(c => c.Car.NumberOfStays >= 2)
                .Include(c => c.Car)
                .ToList();
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

        public OccupancyNumbersOfTheDay[] GetFreeandEmployeeParkingPlacesCount()
        {
            OccupancyNumbersOfTheDay[] freeplaces = new OccupancyNumbersOfTheDay[7];
            var listOfReservationsForNextWeek = new List<Reservation>();
            int totalNumberOfParkingPlaces = 19;

            for (int i = 0; i < 7; i++)
            {
                listOfReservationsForNextWeek = this.GetInhouseReservations(DateTime.Today.AddDays(i));

                freeplaces[i].NumberOfFreePlaces = totalNumberOfParkingPlaces - listOfReservationsForNextWeek.Count() 
                    + listOfReservationsForNextWeek.Where(r => r.ParkingPlaceId > 19).Count(); // odecet mist ktera jsou nestandardni(staff only) stoji na nich pouze zamestnanci
                freeplaces[i].NumberOfPlacesOccupiedByEmployees = listOfReservationsForNextWeek
                                                                    .Where(r => r.Car.IsEmployee == true).Count();
            }

            return freeplaces;
        }
    }
}