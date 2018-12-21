using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.Models
{
    public struct OccupancyNumbersOfTheDay
    {
        public int NumberOfFreePlaces { get; set; }
        public int NumberOfPlacesOccupiedByEmployees { get; set; }
    }
}