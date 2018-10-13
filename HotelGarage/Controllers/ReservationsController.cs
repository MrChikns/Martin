using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelGarage.Controllers
{
    public class ReservationsController : Controller
    {
        private ApplicationDbContext _context;

        public ReservationsController()
        {
            _context = new ApplicationDbContext();
        }

        // Cars form
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reservation reservation)
        {
                        

            if (!ModelState.IsValid)
            {
                return View("Create", reservation);
            }

            Car car = _context.Cars.FirstOrDefault(c => c.LicensePlate == reservation.LicensePlate);

            if (car == null)
            {
                car = new Car { LicensePlate = reservation.LicensePlate };
                _context.Cars.Add(car);
            }
                       

            reservation.Car = car;
            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}