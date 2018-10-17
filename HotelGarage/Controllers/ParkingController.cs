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
                IEnumerable<ParkingPlace> parkingPlaces = _context.ParkingPlaces
                .Include(s => s.StateOfPlace)
                .Include(r => r.Reservation)
                .Include(c => c.Reservation.Car)
                .ToList();                

            return View(parkingPlaces);
        }

        public ActionResult OutsideParking()
        {
            IList<ParkingPlace> parkingPlaces = _context.ParkingPlaces
            .Include(s => s.StateOfPlace)
            .Include(r => r.Reservation)
            .Include(c => c.Reservation.Car)
            .ToList();

            IList<ParkingPlaceDto> parkingPlaceDtos = new List<ParkingPlaceDto>();

            foreach(var parkingPlace in _context.ParkingPlaces)
            {
                string lPlate = "", departure = "", name = "";

                string sOPlace = parkingPlace.StateOfPlace.Name;

                if(parkingPlace.Reservation != null)
                {
                    lPlate = parkingPlace.Reservation.LicensePlate;
                    departure = parkingPlace.Reservation.Departure.ToShortDateString();
                    name = parkingPlace.Name;
                }

                var ppDto = new ParkingPlaceDto {
                    LicensePlate = lPlate,
                    Departure = departure,
                    Name = name,
                    StateOfPlace = sOPlace 
                };

                parkingPlaceDtos.Add(ppDto);
            }

          //  foreach (var pPlace in parkingPlaces)
            //{ if (pPlace.Reservation == null)
              //      pPlace.Reservation = _context.Reservations.Where(i => i.Id == 15);
                    
                //        } 
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