﻿using HotelGarage.Models;
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
            var res = new InhouseReservation() { ParkingPlaceId = parkPlace };

            return View(res);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(InhouseReservation reservation)
        {
            var parkingPlace = _context.ParkingPlaces.First(p => p.Id == reservation.ParkingPlaceId);

            Car car = _context.Cars.FirstOrDefault(c => c.LicensePlate == reservation.LicensePlate);

            if (car == null)
            {
                car = new Car { LicensePlate = reservation.LicensePlate };
                _context.Cars.Add(car);
            }

            reservation.Car = car;

            //parkingPlace.Reservation = parkPlace.Reservation;

            _context.InhouseReservations.Add(reservation);
            parkingPlace.Reservation = reservation;
            
            _context.SaveChanges();
            
                
            return RedirectToAction("Parking", "Parking");
        }

    }
}