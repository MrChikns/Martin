using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using HotelGarage.Dtos;
using HotelGarage.ViewModels;
using System.Web.UI.WebControls;

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
            
            // prirazeni do listu parkingPlaceDtos
            foreach (var parkingPlace in parkingPlaces)
            {
                

                //prepsani textu do buttonu - odjezd nebo neregistrovan! + volno pro staff
                string sOPlace = parkingPlace.StateOfPlace.Name;
                var id = parkingPlaces.IndexOf(parkingPlace);

                switch (sOPlace) {
                    case "Obsazeno":
                        if (!parkingPlace.Reservation.IsRegistered)
                        {
                            sOPlace = "Neregistrován!";
                            break;
                        }
                        if (parkingPlace.Reservation.Departure.Date == DateTime.Today.Date)
                            sOPlace = "Odjezd";
                        break;
                    case "Volno":
                        if (id >= 19)
                            sOPlace = "Volno Staff";
                            break;
                }

                //vypneni rezervace 
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


            IList<ArrivingReservationDto> arrivingReservationDtos = new List<ArrivingReservationDto>();

            // vypsani dnesnich rezervaci
            var today = DateTime.Today.Date;
            var todaysReservations = _context.Reservations
                .Where(a => DbFunctions.TruncateTime(a.Arrival) == today
                    && a.StateOfReservationId == StateOfReservation.Reserved)
                    .Include(c => c.Car)
                .ToList();

            // prirazeni do listu arrivingReservationDtos
            foreach (var reservation in todaysReservations)
            {
                int id = reservation.Id, parkingPlaceId = reservation.Id;
                string carLicensePlate = reservation.Car.LicensePlate, 
                    carGuestsName = reservation.Car.GuestsName, 
                    parkingPlaceName = "Nepřiřazeno";

                // inicializace prazdneho parkovaciho mista
                if (reservation.ParkingPlaceId != 0)
                {
                    parkingPlaceName = _context.ParkingPlaces
                        .First(p => p.Id == reservation.ParkingPlaceId).Name;
                }

                var aRDto = new ArrivingReservationDto
                {
                    Id = id,
                    CarLicensePlate = carLicensePlate,
                    CarGuestsName = carGuestsName,
                    ParkingPlaceId = parkingPlaceId,
                    ParkingPlaceName = parkingPlaceName
                };

                arrivingReservationDtos.Add(aRDto);
            }

            // naplneni seznamu volnych mist pro rezervaci
            var freePlacesList = _context.ParkingPlaces
                .Where(s => s.StateOfPlaceId == StateOfPlace.Free)
                .Select(n => n.Name)
                .ToList();

            // prirazeni do parking view
            var viewModel = new ParkingViewModel
            {
                ParkingPlaceDtos = parkingPlaceDtos,
                TodaysReservations = arrivingReservationDtos,
                FreeParkingPlaces = freePlacesList
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
//                pPlace.StateOfPlace = _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Occupied);
                

                pPlace.Reservation = res;

                _context.SaveChanges();
            }


            return RedirectToAction("Parking");
        }

        public ActionResult CheckOut(int pPlaceId)
        {
            ParkingPlace pPlace = _context.ParkingPlaces.Include(r => r.Reservation).First(p => p.Id == pPlaceId);
            Reservation reservation = pPlace.Reservation;

            reservation.Departure = DateTime.Now;
            reservation.ParkingPlaceId = 0;
            reservation.StateOfReservationId = StateOfReservation.Departed;

            pPlace.Reservation = null;
            pPlace.StateOfPlaceId = StateOfPlace.Free;

            _context.SaveChanges();

            return RedirectToAction("Parking");
        }

        [HttpPost]
        public ActionResult Reserve(string ParkingPlaceName, int ReservationId)
        {

            var pPlace = _context.ParkingPlaces.First(p => p.Name == ParkingPlaceName);
            var reservation = _context.Reservations.First(r => r.Id == ReservationId);

            // presunuti rezervace z predchoziho prirazeneho mista(pokud uz byla nekde prirazena)
            if (reservation.ParkingPlaceId != 0)
            {
                var previousPPlace = _context.ParkingPlaces.First(p => p.Id == reservation.ParkingPlaceId);

                previousPPlace.StateOfPlaceId = StateOfPlace.Free;
                previousPPlace.Reservation = null;
                previousPPlace.StateOfPlace = _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Free);
            }


            

            pPlace.Reservation = reservation;
            reservation.ParkingPlaceId = pPlace.Id;
            pPlace.StateOfPlaceId = StateOfPlace.Reserved;
            pPlace.StateOfPlace = _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Reserved);

            _context.SaveChanges();
            
            return RedirectToAction("Parking");
        }
    }
}