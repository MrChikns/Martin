using HotelGarage.Models;
using System.Linq;

namespace HotelGarage.Repositories
{
    public class CarRepository
    {
        private ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Car GetCar(ParkingPlace parkingPlace)
        {
            return _context.Cars.FirstOrDefault(c => c.LicensePlate == parkingPlace.Reservation.LicensePlate);
        }

        public Car GetCar(Reservation reservation)
        {
            return _context.Cars.FirstOrDefault(c => c.LicensePlate == reservation.Car.LicensePlate);
        }
    }
}