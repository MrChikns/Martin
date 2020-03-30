﻿using HotelGarage.Core.Model;
using HotelGarage.Core.Repository;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HotelGarage.Persistence.Repository
{
    public class ParkingPlaceRepository : IParkingPlaceRepository
    {
        private readonly IApplicationDbContext _context;

        public ParkingPlaceRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public ParkingPlace GetParkingPlace(int id, bool includeCarAndReservation)
        {
            if (includeCarAndReservation)
            {
                return _context.ParkingPlaces
                    .Include(r => r.Reservation)
                    .Include(c => c.Reservation.Car)
                    .FirstOrDefault(p => p.Id == id);
            }

            return _context.ParkingPlaces.FirstOrDefault(p => p.Id == id);
        }

        public List<ParkingPlace> GetParkingPlaces(ParkingPlaceType type, ParkingPlaceType type2)
        {
            return _context.ParkingPlaces.Where(p => p.Type == type || p.Type == type2).ToList();
        }

        public ParkingPlace GetParkingPlace(string name)
        {
            return _context.ParkingPlaces.FirstOrDefault(p => p.Name == name);
        }

        public List<ParkingPlace> GetAllParkingPlaces()
        {
            return _context.ParkingPlaces
                .Include(r => r.Reservation)
                .Include(c => c.Reservation.Car)
                .ToList();
        }

        public List<string> GetFreeParkingPlaceNames()
        {
            return _context.ParkingPlaces
                .Where(s => s.State == ParkingPlaceState.Free)
                .Select(n => n.Name)
                .ToList();
        }

        public string GetParkingPlaceName(int id)
        {
            var parkingPlace = _context.ParkingPlaces.FirstOrDefault(p => p.Id == id);

            return parkingPlace.Name;
        }
    }
}