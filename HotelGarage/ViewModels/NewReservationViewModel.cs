using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.ViewModels
{
    public class NewReservationViewModel
    {
        public Reservation Reservation { get; set; }
        public Car Car { get; set; }
    }
}