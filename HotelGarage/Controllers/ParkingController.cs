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
using System.Collections.ObjectModel;

namespace HotelGarage.Controllers
{
    public class ParkingController : Controller
    {
        private ApplicationDbContext _context;
        private StateOfPlace freePlaceState, employeePlaceState, occupiedPlaceState, reservedPlaceState;

        public ParkingController()
        {
            _context = new ApplicationDbContext();
            freePlaceState = _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Free);
            employeePlaceState = _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Employee);
            occupiedPlaceState = _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Occupied);
            reservedPlaceState = _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Reserved);
        }

        public ActionResult Parking()
        {
            IList<ParkingPlaceDto> parkingPlaceDtos = new List<ParkingPlaceDto>();

            var parkingPlaces = _context.ParkingPlaces
                .Include(s => s.StateOfPlace)
                .Include(r => r.Reservation)
                .ToList();

            // prirazeni do listu vsech parkingPlaceDtos
            foreach (var parkingPlace in parkingPlaces)
            {
                //predvyplneni pro prázdné parkovací místo 
                var ppDto = new ParkingPlaceDto(parkingPlace.Id, 0, " ", " ", parkingPlace.Name,
                    ParkingPlace.AssignStateOfPlaceName(parkingPlace, parkingPlaces.IndexOf(parkingPlace)),
                    " ", " ", 0, " ", 0, "Host");

                // pokud je potreba vyplnit rezervaci do parkovaciho mista
                if (parkingPlace.Reservation != null)
                {
                    ppDto.AssignCar(_context.Cars.FirstOrDefault(c => c.LicensePlate == parkingPlace.Reservation.LicensePlate));
                    ppDto.AssignReservation(parkingPlace);
                }

                parkingPlaceDtos.Add(ppDto);
            }

            IList<ArrivingReservationDto> arrivingReservationDtos = new List<ArrivingReservationDto>();

            // vypsani dnesnich rezervaci
            var todaysReservations = _context.Reservations
                .Where(a => DbFunctions.TruncateTime(a.Arrival) == DateTime.Today.Date
                    && a.StateOfReservationId == StateOfReservation.Reserved)
                    .Include(c => c.Car)
                .ToList();

            // prirazeni do listu arrivingReservationDtos
            foreach (var reservation in todaysReservations)
            {
                var parkingPlaceName = "Nepřiřazeno";

                // inicializace prazdneho parkovaciho mista
                if (reservation.ParkingPlaceId != 0)
                {
                    parkingPlaceName = _context.ParkingPlaces
                        .First(p => p.Id == reservation.ParkingPlaceId).Name;
                }

                var aRDto = new ArrivingReservationDto(reservation.Id, reservation.Car.LicensePlate,
                    reservation.Car.GuestsName, reservation.ParkingPlaceId, parkingPlaceName);

                arrivingReservationDtos.Add(aRDto);
            }

            // naplneni seznamu volnych mist pro rezervaci
            var freePlacesList = _context.ParkingPlaces
                .Where(s => s.StateOfPlaceId == StateOfPlace.Free)
                .Select(n => n.Name)
                .ToList();

            return View(new ParkingViewModel(
                parkingPlaceDtos, arrivingReservationDtos, freePlacesList));
        }
        
        public ActionResult CheckIn(int pPlaceId, int reservationId)
        {
            var reservation = _context.Reservations.First(r => r.Id == reservationId);
            var pPlace = _context.ParkingPlaces.First(p => p.Id == pPlaceId);

            if (reservation.Arrival.Date != DateTime.Today.Date)
                return RedirectToAction("Parking");

            if (pPlace.StateOfPlaceId == StateOfPlace.Reserved)
            {
                reservation.CheckIn();
                pPlace.Occupy(occupiedPlaceState, reservation);

                _context.SaveChanges();
            }

            return RedirectToAction("Parking");
        }

        public ActionResult CheckOut(int pPlaceId)
        {
            var pPlace = _context.ParkingPlaces.Include(r => r.Reservation).First(p => p.Id == pPlaceId);

            pPlace.Reservation.CheckOut();
            pPlace.Release(freePlaceState);

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
                previousPPlace.Release(freePlaceState);
            }

            reservation.ParkingPlaceId = pPlace.Id;
            pPlace.Reserve(reservedPlaceState, reservation);

            _context.SaveChanges();
            
            return RedirectToAction("Parking");
        }
    }
}