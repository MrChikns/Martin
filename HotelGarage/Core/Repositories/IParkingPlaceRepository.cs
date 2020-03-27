using System.Collections.Generic;
using HotelGarage.Core.Models;

namespace HotelGarage.Core.Repositories
{
    public interface IParkingPlaceRepository
    {
        List<string> GetNamesOfFreeParkingPlaces();
        ParkingPlace GetParkingPlace(int parkingPlaceId);
        ParkingPlace GetParkingPlace(Reservation reservation);
        ParkingPlace GetParkingPlace(string parkingPlaceName);
        string GetParkingPlaceName(int id);
        ParkingPlace GetParkingPlaceReservationCar(int parkingPlaceId);
        List<ParkingPlace> GetParkingPlacesStateOfPlaceReservationCar();
        ParkingPlace GetParkingPlaceStateOfPlace(Reservation reservation);
    }
}