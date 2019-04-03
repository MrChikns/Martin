using HotelGarage.Core.Repositories;
using HotelGarage.Persistence.Repositories;

namespace HotelGarage.Core
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