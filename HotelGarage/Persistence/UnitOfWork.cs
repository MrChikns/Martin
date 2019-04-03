using HotelGarage.Models;
using HotelGarage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IParkingPlaceRepository ParkingPlaces { get; private set; }
        public IReservationRepository Reservations { get; private set; }
        public IStateOfPlaceRepository StatesOfPlaces { get; private set; }
        public ICarRepository Cars { get; private set; }


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