using System.Data.Entity;
using HotelGarage.Core.Models;

namespace HotelGarage.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Car> Cars { get; set; }
        DbSet<ParkingPlace> ParkingPlaces { get; set; }
        DbSet<Reservation> Reservations { get; set; }
        DbSet<StateOfReservation> StateOfReservations { get; set; }
        DbSet<StateOfPlace> StatesOfPlace { get; set; }
    }
}