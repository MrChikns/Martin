﻿using HotelGarage.Models;
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
            var res = new Reservation() { ParkingPlaceId = (pPlaceId != null) ? (int)pPlaceId :  0, StateOfReservationId = StateOfReservation.Reserved};

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
            if (!ModelState.IsValid)
            {
                return View("Form", viewModel);
            }
            
            // prirazeni auta pokud jiz existuje
            var car = _context.Cars.FirstOrDefault(c => c.LicensePlate == viewModel.Car.LicensePlate);
            Reservation reservation;

            // vytvoreni rezervace
            if (viewModel.Id == 0)
            {
                reservation = viewModel;

                // vytvoreni auta pro novou rezervaci
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
                // update auta
                else
                {
                    car.LicensePlate = viewModel.Car.LicensePlate;
                    car.CarModel = viewModel.Car.CarModel;
                    car.GuestsName = viewModel.Car.GuestsName;
                    car.GuestRoomNumber = viewModel.Car.GuestRoomNumber;
                    car.PricePerNight = viewModel.Car.PricePerNight;
                    car.IsEmployee = viewModel.Car.IsEmployee;
                }

                reservation.Car = car;
                reservation.StateOfReservationId = StateOfReservation.Reserved;

                _context.Reservations.Add(reservation);
            }
            //update jiz vytvorene rezervace
            else
            {
                car.LicensePlate = viewModel.Car.LicensePlate;
                car.CarModel = viewModel.Car.CarModel;
                car.GuestsName = viewModel.Car.GuestsName;
                car.GuestRoomNumber = viewModel.Car.GuestRoomNumber;
                car.PricePerNight = viewModel.Car.PricePerNight;
                car.IsEmployee = viewModel.Car.IsEmployee;

                reservation = _context.Reservations.First(r => r.Id == viewModel.Id);

                reservation.Arrival = viewModel.Arrival;
                reservation.Departure = viewModel.Departure;
                reservation.IsRegistered = viewModel.IsRegistered;
                reservation.LicensePlate = viewModel.LicensePlate;
                reservation.ParkingPlaceId = viewModel.ParkingPlaceId;
                reservation.StateOfReservationId = viewModel.StateOfReservationId;
                reservation.Car = car;
            }

            // prirazeni k parkovacimu mistu
            var pPlace = _context.ParkingPlaces.Include(s => s.StateOfPlace).FirstOrDefault(p => p.Id == reservation.ParkingPlaceId);
            
            // prirazene parkovaci misto
            if(reservation.ParkingPlaceId != 0)
            {
                // pokud stav neni inhouse (pro inhouse)
                if (reservation.StateOfReservationId != StateOfReservation.Inhouse)
                { 
                    // pokud je stav rezervovano a prijezd dnes 
                    //=> prirazeni k mistu rezervace a nastaveni mista na rezervovano
                     if (reservation.StateOfReservationId == StateOfReservation.Reserved
                        && reservation.Arrival.DayOfYear == DateTime.Today.DayOfYear)
                    {
                        pPlace.Reservation = reservation;
                        pPlace.StateOfPlaceId = StateOfPlace.Reserved;
                        pPlace.StateOfPlace = _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Reserved);
                    }
                    // ostatni pripady prirazeni prazdneho mista
                    else
                    {
                        reservation.ParkingPlaceId = 0;
                        pPlace.Reservation = null;
                        pPlace.StateOfPlace = _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Free);
                    }
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Parking", "Parking");
        }
    }
}