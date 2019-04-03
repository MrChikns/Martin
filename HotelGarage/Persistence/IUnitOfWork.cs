using HotelGarage.Repositories;

namespace HotelGarage.Persistence
{
    public interface IUnitOfWork
    {
        ICarRepository Cars { get; }
        IParkingPlaceRepository ParkingPlaces { get; }
        IReservationRepository Reservations { get; }
        IStateOfPlaceRepository StatesOfPlaces { get; }

        void Complete();
    }
}