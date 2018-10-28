using HotelGarage.Models;
using HotelGarage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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

        // nova rezervace
        public ActionResult Create(int? pPlaceId)
        {
            var res = new Reservation() { ParkingPlaceId = (pPlaceId != null) ? (int)pPlaceId :  0, LicensePlate = "Doplňte" };

            return View("Form", res);
        }

        // update rezervace
        public ActionResult Update(int resId)
        {
            var res = _context.Reservations.Include(c => c.Car).First(r => r.Id == resId);

            return View("Form", res);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Reservation viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", viewModel);
            }


            var car = _context.Cars.FirstOrDefault(c => c.LicensePlate == viewModel.Car.LicensePlate);
            Reservation reservation;

            if (viewModel.Id == 0)
            {
                reservation = viewModel;
                //nova rezervace
                if (car == null)
                {
                    car = new Car
                    {
                        LicensePlate = viewModel.Car.LicensePlate,
                        CarModel = viewModel.Car.CarModel,
                        GuestsName = viewModel.Car.GuestsName,
                        GuestRoomNumber = viewModel.Car.GuestRoomNumber,
                        PricePerNight = viewModel.Car.PricePerNight,
                        IsEmployee = viewModel.Car.IsEmployee
                    };

                    _context.Cars.Add(car);
                }

                viewModel.Car = car;
                viewModel.StateOfReservationId = StateOfReservation.Reserved;

                _context.Reservations.Add(viewModel);
            }
            else
            {
                // update rezervace
                reservation = _context.Reservations.First(r => r.Id == viewModel.Id);

                reservation.Arrival = viewModel.Arrival;
                reservation.Departure = viewModel.Departure;
                reservation.IsRegistered = viewModel.IsRegistered;
                reservation.LicensePlate = viewModel.LicensePlate;
                reservation.ParkingPlaceId = viewModel.ParkingPlaceId;
                reservation.StateOfReservationId = viewModel.StateOfReservationId;
                reservation.Car = viewModel.Car;
            }

            // prirazeni 
            var pPlace = _context.ParkingPlaces.First(p => p.Id == viewModel.ParkingPlaceId);
            // pokud je prijezd dnes - nastaveni parkovaciho mista
            if (viewModel.Arrival.DayOfYear == DateTime.Today.DayOfYear)
            {
                pPlace.Reservation = viewModel;
                pPlace.StateOfPlaceId = StateOfPlace.Reserved;
                pPlace.StateOfPlace = _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Reserved);
            }
            //pokud prijezd neni dnes - vymazani parkovaciho mista
            else
            {
                reservation.ParkingPlaceId = 0;
                pPlace.Reservation = null;
                pPlace.StateOfPlace = _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Free);
            }

            _context.SaveChanges();

            return RedirectToAction("Parking", "Parking");
        }

        public ActionResult CheckIn()
        {

            return View();
        }

    }
}