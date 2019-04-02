using HotelGarage.Models;
using System;
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

        public Car GetCar(Reservation reservation)
        {
            return _context.Cars.FirstOrDefault(c => c.LicensePlate == reservation.Car.LicensePlate);
        }

        internal void AddCar(Car car)
        {
            _context.Cars.Add(car);
        }
    }
}