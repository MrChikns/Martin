﻿using System.Collections.Generic;
using HotelGarage.Core.Models;

namespace HotelGarage.Core.Repositories
{
    public interface IParkingPlaceRepository
    {
        ParkingPlace GetParkingPlace(int id, bool includeCarAndReservation);
        ParkingPlace GetParkingPlace(string name);
        List<ParkingPlace> GetAllParkingPlaces();
        List<string> GetFreeParkingPlaceNames();
        string GetParkingPlaceName(int id);
    }
}