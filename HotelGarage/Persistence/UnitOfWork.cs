using HotelGarage.Models;
using HotelGarage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.Persistence
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ParkingPlaceRepository ParkingPlaces { get; private set; }
        public ReservationRepository Reservations { get; private set; }
        public StateOfPlaceRepository StatesOfPlaces { get; private set; }
        public CarRepository Cars { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ParkingPlaces = new ParkingPlaceRepository(_context);
            Reservations = new ReservationRepository(_context);
            StatesOfPlaces = new StateOfPlaceRepository(_context); 
            Cars = new CarRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}