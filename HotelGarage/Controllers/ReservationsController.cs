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
        public ActionResult Create(int? pPlaceId)
        {
            var res = new Reservation() { ParkingPlaceId = (pPlaceId != null) ? (int)pPlaceId :  0 };

            return View(res);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reservation viewModel)
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

            // prirazeni k parkovacimu mistu pouze pokud je prijezd dnes
            if (viewModel.Arrival.DayOfYear == DateTime.Today.DayOfYear)
            {
                var pPlace = _context.ParkingPlaces.First(p => p.Id == viewModel.ParkingPlaceId);
                pPlace.Reservation = viewModel;
                pPlace.StateOfPlaceId = StateOfPlace.Reserved;
                pPlace.StateOfPlace = _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Reserved);
            }
            else
            {
                //vymazani pPlace Id, protoze prijezd neni dnes
                viewModel.ParkingPlaceId = 0;
            }

            viewModel.Car = car;
            viewModel.StateOfReservationId = StateOfReservation.Reserved;

            _context.Reservations.Add(viewModel);
            _context.SaveChanges();

            return RedirectToAction("Parking", "Parking");
        }

        public ActionResult CheckIn()
        {

            return View();
        }

    }
}