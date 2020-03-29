using HotelGarage.Core.Models;
using HotelGarage.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HotelGarage.Persistence.Repositories
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