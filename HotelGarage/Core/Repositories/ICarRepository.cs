using HotelGarage.Core.Models;

namespace HotelGarage.Core.Repositories
{
    public interface ICarRepository
    {
        Car GetCar(Reservation reservation);

        void Add(Car car);
    }
}