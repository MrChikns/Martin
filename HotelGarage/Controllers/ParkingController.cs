using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace HotelGarage.Controllers
{
    public class ParkingController : Controller
    {
        private ApplicationDbContext _context;

        public ParkingController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Parking()
        {
            var parkingPlaces = _context.ParkingPlaces
                .Include(s => s.StateOfPlace)
                .ToList();

            return View(parkingPlaces);
        }

       public ActionResult CheckIn(ActualReservation reservation)
        {


            var parkingPlaces = _context.ParkingPlaces
                .Include(s => s.StateOfPlace)
                .ToList();

            return View(parkingPlaces);
        }
    }
}