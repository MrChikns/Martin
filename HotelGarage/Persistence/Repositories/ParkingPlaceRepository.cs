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

        public ParkingPlace GetParkingPlace(string ParkingPlaceName)
        {
            return _context.ParkingPlaces.FirstOrDefault(p => p.Name == ParkingPlaceName);
        }

        public ParkingPlace GetParkingPlace(int parkingPlaceId)
        {
            return _context.ParkingPlaces.FirstOrDefault(p => p.Id == parkingPlaceId);
        }

        public ParkingPlace GetParkingPlaceReservationCar(int parkingPlaceId)
        {
            return _context.ParkingPlaces
                .Include(r => r.Reservation)
                .Include(c => c.Reservation.Car)
                .First(p => p.Id == parkingPlaceId);
        }

        public ParkingPlace GetParkingPlace(Reservation reservation)
        {
            return _context.ParkingPlaces.FirstOrDefault(p => p.Id == reservation.ParkingPlaceId);
        }

        public ParkingPlace GetParkingPlaceStateOfPlace(Reservation reservation)
        {
            return _context.ParkingPlaces
                .Include(s => s.State)
                .FirstOrDefault(p => p.Id == reservation.ParkingPlaceId);
        }

        public List<ParkingPlace> GetParkingPlacesStateOfPlaceReservationCar()
        {
            return _context.ParkingPlaces
                .Include(s => s.State)
                .Include(r => r.Reservation)
                .Include(c => c.Reservation.Car)
                .ToList();
        }

        public List<string> GetNamesOfFreeParkingPlaces()
        {
            return _context.ParkingPlaces
                .Where(s => s.State == StateOfPlaceEnum.Free)
                .Select(n => n.Name)
                .ToList();
        }

        public string GetParkingPlaceName(int id)
        {
            var parkingPlace = _context.ParkingPlaces.FirstOrDefault(p => p.Id == id);

            if (parkingPlace == null)
                return null;

            return parkingPlace.Name;
        }
    }
}