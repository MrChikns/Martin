using System.Collections.Generic;
using HotelGarage.Core.Model;

namespace HotelGarage.Core.Repository
{
    public interface IParkingPlaceRepository
    {
        ParkingPlace GetParkingPlace(int id, bool includeCarAndReservation);
        ParkingPlace GetParkingPlace(string name);
        List<ParkingPlace> GetAllParkingPlaces();
        List<ParkingPlace> GetParkingPlaces(ParkingPlaceType type, ParkingPlaceType type2);
        List<string> GetFreeParkingPlaceNames();
        string GetParkingPlaceName(int id);
    }
}