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

            var cars = _context.Cars.ToList();

            // prirazeni do listu parkingPlaceDtos
            foreach (var parkingPlace in parkingPlaces)
            {
                //prepsani textu do buttonu - odjezd nebo neregistrovan! + volno pro staff
                string stateOfPlaceName = ParkingPlace.AssignStateOfPlaceName(parkingPlace.StateOfPlace.Name,
                    parkingPlace, parkingPlaces.IndexOf(parkingPlace));

                //vypneni rezervace 
                string licensePlate = " ", departure = " ", arrival = " ", pPlaceName = parkingPlace.Name,
                    pPlaceGuestsName = " ", pPlaceCar = " ", isEmployee = " ";
                int? resId = 0, pPPrice = 0, pPRoom = 0;
                Car car = null;



                // pokud je potreba vyplnit rezervaci do parkovaciho mista
                if (parkingPlace.Reservation != null)
                {
                    car = cars.FirstOrDefault(c => c.LicensePlate == parkingPlace.Reservation.LicensePlate);

                    if (car != null)
                    {
                        pPlaceGuestsName = (car.GuestsName == null) ? "Nevyplněno" : car.GuestsName;
                        pPRoom = (car.GuestRoomNumber == null) ? 0 : car.GuestRoomNumber;
                        pPlaceCar = (car.CarModel == null) ? "Nevyplněno" : car.CarModel;
                        pPPrice = (car.PricePerNight == null) ? 0 : car.PricePerNight;
                        isEmployee = (car.IsEmployee == true) ? "Zaměstnanec" : "Host";
                    }

                    licensePlate = parkingPlace.Reservation.LicensePlate;
                    departure = parkingPlace.Reservation.Departure.ToLongDateString();
                    arrival = parkingPlace.Reservation.Arrival.ToLongDateString();
                    resId = parkingPlace.Reservation.Id;
                }

                var ppDto = new ParkingPlaceDto(parkingPlace.Id, resId, licensePlate,
                    departure, pPlaceName, stateOfPlaceName, arrival, pPlaceGuestsName,
                    pPRoom, pPlaceCar, pPPrice, isEmployee);

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

            // prirazeni do parking view
            var viewModel = new ParkingViewModel(
                parkingPlaceDtos, arrivingReservationDtos, freePlacesList); 

            return View(viewModel);
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