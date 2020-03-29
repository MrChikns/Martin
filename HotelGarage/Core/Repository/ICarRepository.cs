using HotelGarage.Core.Model;

namespace HotelGarage.Core.Repository
{
    public interface ICarRepository
    {
        Car GetCar(Reservation reservation);

        void Add(Car car);
    }
}