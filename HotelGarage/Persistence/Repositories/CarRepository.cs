using HotelGarage.Core.Models;
using HotelGarage.Core.Repositories;
using HotelGarage.Persistence;
using System;
using System.Linq;

namespace HotelGarage.Persistence.Repositories
{
    public class CarRepository : ICarRepository
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

        public void Add(Car car)
        {
            _context.Cars.Add(car);
        }
    }
}