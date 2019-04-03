using HotelGarage.Core.Models;

namespace HotelGarage.Core.Repositories
{
    public interface IStateOfPlaceRepository
    {
        StateOfPlace GetFreeStateOfPlace();
        StateOfPlace GetOccupiedStateOfPlace();
        StateOfPlace GetReservedStateOfPlace();
    }
}