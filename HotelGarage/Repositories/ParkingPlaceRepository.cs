using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace HotelGarage.Repositories
{
    public class ParkingPlaceRepository
    {
        private readonly ApplicationDbContext _context;

        public ParkingPlaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ParkingPlace GetParkingPlace(string ParkingPlaceName)
        {
            return _context.ParkingPlaces.FirstOrDefault(p => p.Name == ParkingPlaceName);
        }

        public ParkingPlace GetParkingPlace(int pPlaceId)
        {
            return _context.ParkingPlaces.FirstOrDefault(p => p.Id == pPlaceId);
        }

        public ParkingPlace GetParkingPlaceReservation(int pPlaceId)
        {
            return _context.ParkingPlaces.Include(r => r.Reservation).First(p => p.Id == pPlaceId);
        }

        public ParkingPlace GetParkingPlace(Reservation reservation)
        {
            return _context.ParkingPlaces.First(p => p.Id == reservation.ParkingPlaceId);
        }

        public ParkingPlace GetParkingPlaceStateOfPlace(Reservation reservation)
        {
            return _context.ParkingPlaces.Include(s => s.StateOfPlace).FirstOrDefault(p => p.Id == reservation.ParkingPlaceId);
        }

        public List<ParkingPlace> GetParkingPlacesStateOfPlaceReservationCar()
        {
            return _context.ParkingPlaces
                .Include(s => s.StateOfPlace)
                .Include(r => r.Reservation)
                .Include(c => c.Reservation.Car)
                .ToList();
        }

        public List<string> GetNamesOfFreeParkingPlaces()
        {
            return _context.ParkingPlaces
                .Where(s => s.StateOfPlaceId == StateOfPlace.Free)
                .Select(n => n.Name)
                .ToList();
        }

        public string GetParkingPlaceName(int id)
        {
            return _context.ParkingPlaces.First(p => p.Id == id).Name;
        }
    }
}