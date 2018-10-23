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

                pPlace.Reservation = res;

                _context.SaveChanges();
            }


            return RedirectToAction("Parking");
        }

        public class ReserveDto
        {
            public int ResId { get; set; }
            public int PPlace { get; set; }
        }

        [HttpPost]
        public ActionResult Reserve(string ParkingPlaceName, int ReservationId)
        {

            ParkingPlace pPlace = _context.ParkingPlaces.First(p => p.Name == ParkingPlaceName);
            Reservation reservation = _context.Reservations.First(r => r.Id == ReservationId);

            pPlace.Reservation = reservation;
            reservation.ParkingPlaceId = pPlace.Id;

            _context.SaveChanges();
            
            return RedirectToAction("Parking");
        }
    }
}