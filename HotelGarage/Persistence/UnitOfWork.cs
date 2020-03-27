using HotelGarage.Core;
using HotelGarage.Core.Repositories;
using HotelGarage.Persistence.Repositories;

namespace HotelGarage.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IParkingPlaceRepository ParkingPlaces { get; private set; }
        public IReservationRepository Reservations { get; private set; }
        public ICarRepository Cars { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ParkingPlaces = new ParkingPlaceRepository(_context);
            Reservations = new ReservationRepository(_context);
            Cars = new CarRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}