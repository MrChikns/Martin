using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.ViewModels
{
    public class CheckInViewModel
    {
        public int id { get; set; }
        public InhouseReservation InhouseReservation { get; set; }

    }
}