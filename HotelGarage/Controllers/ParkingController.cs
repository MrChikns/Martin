﻿using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using HotelGarage.Dtos;
using HotelGarage.ViewModels;

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
            IList<ParkingPlaceDto> parkingPlaceDtos = new List<ParkingPlaceDto>();

            var parkingPlaces = _context.ParkingPlaces
                .Include(s => s.StateOfPlace)
                .Include(r => r.Reservation)
                .ToList();

            foreach (var parkingPlace in parkingPlaces)
            {
                

                //prepsani textu do buttonu - odjezd nebo neregistrovan! + volno pro staff
                string sOPlace = parkingPlace.StateOfPlace.Name;
                var id = parkingPlaces.IndexOf(parkingPlace);

                switch (sOPlace) {
                    case "Obsazeno":
                        if (parkingPlace.Reservation.Departure.Date == DateTime.Today.Date)
                            sOPlace = "Odjezd";
                        if (parkingPlace.Reservation.Departure.Date == DateTime.Today.Date && !parkingPlace.Reservation.IsRegistered)
                            sOPlace = "Neregistrován!";
                        break;
                    case "Volno":
                        if (id >= 19)
                            sOPlace = "Volno Staff";
                            break;
                }

                //vypneni rezervace pro prázné parkovací místo
                string lPlate = "", departure = "", pPName = parkingPlace.Name;
                int? resId = null;

                if (parkingPlace.Reservation != null)
                {
                    lPlate = parkingPlace.Reservation.LicensePlate;
                    departure = parkingPlace.Reservation.Departure.ToShortDateString();
                    pPName = parkingPlace.Name;
                    resId = parkingPlace.Reservation.Id;
                }

                var ppDto = new ParkingPlaceDto
                {
                    Id = parkingPlace.Id,
                    ReservationId = resId,
                    LicensePlate = lPlate,
                    Departure = departure,
                    PPlaceName = pPName,
                    StateOfPlace = sOPlace
                };

                

                parkingPlaceDtos.Add(ppDto);
            }

            // vypsani dnesnich rezervaci
            var today = DateTime.Today.Date;
            var todaysReservations = _context.Reservations
                .Where(a => DbFunctions.TruncateTime(a.Arrival) == today
                    && a.StateOfReservationId == StateOfReservation.Reserved)
                    .Include(c => c.Car)
                .ToList();

            
            // prirazeni do parking view
            var viewModel = new ParkingViewModel
            {
                ParkingPlaceDtos = parkingPlaceDtos,
                TodaysReservations = todaysReservations
            }; 

            return View(viewModel);
        }
        
        public ActionResult CheckIn(int pPlaceId, int reservationId)
        {
            ParkingPlace pPlace = _context.ParkingPlaces.First(p => p.Id == pPlaceId);

            if (pPlace.StateOfPlaceId == StateOfPlace.Reserved)
            {
                Reservation res = _context.Reservations.First(r => r.Id == reservationId);

                res.StateOfReservationId = StateOfReservation.Inhouse;
                pPlace.StateOfPlaceId = StateOfPlace.Occupied;

                pPlace.Reservation = res;

                _context.SaveChanges();
            }


            return RedirectToAction("Parking");
        }
    }
}