using HotelGarage.Models;

namespace HotelGarage.Repositories
{
    public interface IStateOfPlaceRepository
    {
        StateOfPlace GetFreeStateOfPlace();
        StateOfPlace GetOccupiedStateOfPlace();
        StateOfPlace GetReservedStateOfPlace();
    }
}