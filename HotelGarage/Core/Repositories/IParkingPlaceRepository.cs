using System.Collections.Generic;
using HotelGarage.Core.Models;

namespace HotelGarage.Core.Repositories
{
    public interface IParkingPlaceRepository
    {
        ParkingPlace GetParkingPlace(int id);
        ParkingPlace GetParkingPlace(string name);
        ParkingPlace GetParkingPlace(Reservation reservation);
        List<ParkingPlace> GetParkingPlaces();
        List<string> GetFreeParkingPlaceNames();
        string GetParkingPlaceName(int id);
    }
}