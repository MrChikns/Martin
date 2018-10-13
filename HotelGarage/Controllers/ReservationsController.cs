using HotelGarage.Models;
using HotelGarage.ViewModels;
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
        public ActionResult Create(ActualReservation reservation)
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
            _context.ActualReservations.Add(reservation);
            _context.SaveChanges();

            return RedirectToAction("Parking", "Parking");
        }

        public ActionResult CheckIn(int parkPlace)
        {
            CheckInViewModel viewModel = new CheckInViewModel() { id = parkPlace };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(CheckInViewModel viewModel)
        {
            var parkingPlace = _context.ParkingPlaces.First(p => viewModel.id == p.Id);

            parkingPlace.Reservation = viewModel.InhouseReservation;
                
            return RedirectToAction("Parking", "Parking");
        }

    }
}