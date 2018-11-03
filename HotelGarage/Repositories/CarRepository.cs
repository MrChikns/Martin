using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public Car GetCar(Reservation viewModel)
        {
            return _context.Cars.FirstOrDefault(c => c.LicensePlate == viewModel.Car.LicensePlate);
        }
    }
}