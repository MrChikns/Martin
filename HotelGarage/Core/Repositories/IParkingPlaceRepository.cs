using System.Collections.Generic;
using HotelGarage.Core.Models;

namespace HotelGarage.Core.Repositories
{
    public interface IParkingPlaceRepository
    {
        List<string> GetNamesOfFreeParkingPlaces();
        ParkingPlace GetParkingPlace(int pPlaceId);
        ParkingPlace GetParkingPlace(Reservation reservation);
        ParkingPlace GetParkingPlace(string ParkingPlaceName);
        string GetParkingPlaceName(int id);
        ParkingPlace GetParkingPlaceReservationCar(int pPlaceId);
        List<ParkingPlace> GetParkingPlacesStateOfPlaceReservationCar();
        ParkingPlace GetParkingPlaceStateOfPlace(Reservation reservation);
    }
}