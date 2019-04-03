using HotelGarage.Models;

namespace HotelGarage.Repositories
{
    public interface ICarRepository
    {
        Car GetCar(Reservation reservation);

        void Add(Car car);
    }
}