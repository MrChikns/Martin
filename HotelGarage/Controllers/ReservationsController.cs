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
            var res = new Reservation() { ParkingPlaceId = (pPlaceId != null) 
                ? (int)pPlaceId :  0, StateOfReservationId = StateOfReservation.Reserved};

            return View("Form", res);
        }

        // update rezervace
        public ActionResult Update(int resId)
        {
            var res = _context.Reservations.Include(c => c.Car).First(r => r.Id == resId);

            return View("Form", res);
        }
    
        // ulozeni nove nebo upravovane rezervace a auta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Reservation viewModel)
        {
            // prirazeni auta a rezervace pokud jiz existuji a nastaveni jejich atributu
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == viewModel.Id); ;
            var car = _context.Cars.FirstOrDefault(c => c.LicensePlate == viewModel.Car.LicensePlate);

            if (!ModelState.IsValid)
                return View("Form", viewModel);

            // pridani auta do DB anebo update
            if (car == null)
            {
                car = new Car(viewModel.Car.LicensePlate, viewModel.Car.CarModel, viewModel.Car.GuestsName,
                      viewModel.Car.GuestRoomNumber, viewModel.Car.PricePerNight, viewModel.Car.IsEmployee);

                _context.Cars.Add(car);
            }
            else
                car.Update(viewModel);

            //vytvoreni rezervace anebo update
            if (viewModel.Id == 0)
            {
                reservation = new Reservation(viewModel.LicensePlate, viewModel.Arrival, viewModel.Departure,
                    viewModel.IsRegistered, viewModel.ParkingPlaceId, car);

                _context.Reservations.Add(reservation);
            }
            else 
                reservation.Update(viewModel, car);

            // prirazeni k parkovacimu mistu
            var pPlace = _context.ParkingPlaces.Include(s => s.StateOfPlace).FirstOrDefault(p => p.Id == reservation.ParkingPlaceId);

            // pokud je prirazene parkovaci misto a rezervace neni inhouse
            if (reservation.ParkingPlaceId != 0 && reservation.StateOfReservationId != StateOfReservation.Inhouse)
            {                
                // prirazeni k mistu rezervace a nastaveni mista na rezervovano
                if (reservation.StateOfReservationId == StateOfReservation.Reserved
                    && reservation.Arrival.DayOfYear == DateTime.Today.DayOfYear)
                    pPlace.Reserve(_context.StatesOfPlace.First(s => s.Id == StateOfPlace.Reserved), reservation);
                // anebo prirazeni prazdneho park. mista
                else
                    pPlace.Free(_context.StatesOfPlace.First(s => s.Id == StateOfPlace.Free), reservation);
            }
            _context.SaveChanges();

            return RedirectToAction("Parking", "Parking");
        }
    }
}