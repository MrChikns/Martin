using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.ViewModels
{
    public class ParkingViewModel
    {
        public IEnumerable<ParkingPlace> ParkingPlaces { get;set;}
        public IEnumerable<InhouseReservation> InhouseReservations { get; set; }
        public IEnumerable<Car> Cars { get; set; }
    }
}