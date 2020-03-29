using System.Data.Entity;
using HotelGarage.Core.Model;

namespace HotelGarage.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Car> Cars { get; set; }
        DbSet<ParkingPlace> ParkingPlaces { get; set; }
        DbSet<Reservation> Reservations { get; set; }
    }
}