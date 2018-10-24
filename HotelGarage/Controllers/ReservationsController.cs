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
        public ActionResult Create(NewReservationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", viewModel);
            }

            Car car = _context.Cars.FirstOrDefault(c => c.LicensePlate == viewModel.Car.LicensePlate);

            if (car == null)
            {
                car = new Car { LicensePlate = viewModel.Car.LicensePlate ,
                    CarModel = viewModel.Car.CarModel,
                    GuestsName = viewModel.Car.GuestsName,
                    GuestRoomNumber = viewModel.Car.GuestRoomNumber,
                    PricePerNight = viewModel.Car.PricePerNight,
                    IsEmployee = viewModel.Car.IsEmployee
                };
                _context.Cars.Add(car);
            }

            viewModel.Reservation.Car = car;
            viewModel.Reservation.StateOfReservationId = StateOfReservation.Reserved;

            _context.Reservations.Add(viewModel.Reservation);
            _context.SaveChanges();

            return RedirectToAction("Parking", "Parking");
        }

        public ActionResult CheckIn()
        {

            return View();
        }

    }
}