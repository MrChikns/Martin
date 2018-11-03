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
using HotelGarage.Repositories;

namespace HotelGarage.Controllers
{
    public class ParkingController : Controller
    {
        private ApplicationDbContext _context;
        private List<StateOfPlace> listOfPlaceStates;
        private readonly ParkingPlaceRepository _parkingPlaceRepository;
        private readonly ReservationRepository _reservationRepository;
        private readonly CarRepository _carRepository;
        private readonly StateOfPlaceRepository _stateOfPlaceRepository;

        public ParkingController()
        {
            _context = new ApplicationDbContext();
            _stateOfPlaceRepository = new StateOfPlaceRepository(_context);
            _parkingPlaceRepository = new ParkingPlaceRepository(_context);
            _reservationRepository = new ReservationRepository(_context);
            _carRepository = new CarRepository(_context);
            listOfPlaceStates = _stateOfPlaceRepository.GetStatesOfPlace();
        }

        public ActionResult Parking()
        {
            IList<ParkingPlaceDto> parkingPlaceDtos = new List<ParkingPlaceDto>();
            foreach (var parkingPlace in _parkingPlaceRepository.GetParkingPlacesStateOfPlaceReservation())
            {

                //predvyplneni pro prázdné parkovací místo 
                var ppDto = new ParkingPlaceDto(parkingPlace.Id, 0, " ", " ", parkingPlace.Name,
                    ParkingPlace.AssignStateOfPlaceName(parkingPlace, 
                    _parkingPlaceRepository.GetParkingPlacesStateOfPlaceReservation().IndexOf(parkingPlace)),
                    " ", " ", 0, " ", 0, "Host");

                // pokud je potreba vyplnit rezervaci do parkovaciho mista
                if (parkingPlace.Reservation != null)
                {
                    //vyrazeni rezervaci z minuleho dne anebo prirazeni rezervace do parkovaciho mista
                    if (parkingPlace.Reservation.Arrival.Date != DateTime.Today.Date
                        && parkingPlace.Reservation.StateOfReservationId == StateOfReservation.Reserved)
                        parkingPlace.Release(listOfPlaceStates.First(s => s.Id == StateOfPlace.Free));
                    else
                    {
                        ppDto.AssignCar(_carRepository.GetCar(parkingPlace));
                        ppDto.AssignReservation(parkingPlace);
                    }
                }

                parkingPlaceDtos.Add(ppDto);
            }

            IList<ReservationDto> arrivingReservationDtos = new List<ReservationDto>();
            foreach (var reservation in _reservationRepository.GetTodaysReservationsCar())
            {
                string parkingPlaceName;

                if (reservation.ParkingPlaceId != 0)
                    parkingPlaceName = _parkingPlaceRepository.GetParkingPlace(reservation).Name;
                else
                    parkingPlaceName = "Nepřiřazeno";

                arrivingReservationDtos.Add(new ReservationDto(reservation.Id, reservation.Car.LicensePlate,
                    reservation.Car.GuestsName, reservation.ParkingPlaceId, parkingPlaceName, 
                    reservation.Arrival.ToShortDateString()));
            }

            IList<ReservationDto> noShowReservationDtos = new List<ReservationDto>();
            foreach (var reservation in _reservationRepository.GetNoShowReservationsCar())
            {
                //asi smazat
                string parkingPlaceName;

                if (reservation.ParkingPlaceId != 0)
                    parkingPlaceName = _parkingPlaceRepository.GetParkingPlace(reservation).Name;
                else
                    parkingPlaceName = "Nepřiřazeno";

                noShowReservationDtos.Add(new ReservationDto(reservation.Id, reservation.Car.LicensePlate,
                    reservation.Car.GuestsName, reservation.ParkingPlaceId, parkingPlaceName, 
                    reservation.Arrival.ToShortDateString()));
            }


            return View(new ParkingViewModel(
                parkingPlaceDtos, arrivingReservationDtos,noShowReservationDtos , _parkingPlaceRepository.GetNamesOfFreeParkingPlaces()));
        }

        
        public ActionResult CheckIn(int pPlaceId, int reservationId)
        {
            var reservation = _reservationRepository.GetReservation(reservationId);

            if (reservation.Arrival.Date != DateTime.Today.Date)
                return RedirectToAction("Parking");

            if (_parkingPlaceRepository.GetParkingPlace(pPlaceId).StateOfPlaceId == StateOfPlace.Reserved)
            {
                reservation.CheckIn();
                _parkingPlaceRepository.GetParkingPlace(pPlaceId)
                    .Occupy(listOfPlaceStates.First(s => s.Id == StateOfPlace.Occupied), reservation);

                _context.SaveChanges();
            }

            return RedirectToAction("Parking");
        }


        public ActionResult CheckOut(int pPlaceId)
        {
            var pPlace = _parkingPlaceRepository.GetParkingPlaceReservation(pPlaceId);

            pPlace.Reservation.CheckOut();
            pPlace.Release(listOfPlaceStates.First(s => s.Id == StateOfPlace.Free));

            _context.SaveChanges();

            return RedirectToAction("Parking");
        }

        [HttpPost]
        public ActionResult Reserve(string ParkingPlaceName, int ReservationId)
        {
            var reservation = _reservationRepository.GetReservation(ReservationId);

            // presunuti rezervace z predchoziho prirazeneho mista(pokud uz byla nekde prirazena)
            if (reservation.ParkingPlaceId != 0)
            {
                _parkingPlaceRepository.GetParkingPlace(reservation)
                    .Release(listOfPlaceStates.First(s => s.Id == StateOfPlace.Free));
            }
            _parkingPlaceRepository.GetParkingPlace(ParkingPlaceName)
                .Reserve(listOfPlaceStates.First(s => s.Id == StateOfPlace.Reserved), reservation);

            _context.SaveChanges();

            return RedirectToAction("Parking");
        }
    }
}