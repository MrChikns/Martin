using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using HotelGarage.ViewModels;
using HotelGarage.Dtos;

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
                //odjezd a neregistrovan!
                string lPlate = "", departure = "", name = "";
                var id = parkingPlace.StateOfPlaceId;

                string sOPlace = parkingPlace.StateOfPlace.Name;
                switch (sOPlace) {
                    case "Obsazeno":
                        if (parkingPlace.Reservation.Departure.Date == DateTime.Today.Date)
                            sOPlace = "Odjezd";
                        if (parkingPlace.Reservation.Departure.Date == DateTime.Today.Date && !parkingPlace.Reservation.IsRegistered)
                            sOPlace = "Neregistrován!";
                        break;
                    
                          }

                if (parkingPlace.Reservation != null)
                {
                    lPlate = parkingPlace.Reservation.LicensePlate;
                    departure = parkingPlace.Reservation.Departure.ToShortDateString();
                    name = parkingPlace.Name;
                }

                var ppDto = new ParkingPlaceDto
                {
                    LicensePlate = lPlate,
                    Departure = departure,
                    Name = name,
                    StateOfPlace = sOPlace
                };

                parkingPlaceDtos.Add(ppDto);
            }

            return View(parkingPlaceDtos);
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