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
        }

        public ActionResult Parking()
        {
            return View(new ParkingViewModel(
                ParkingPlaceDto.GetParkingPlaceDtos(_parkingPlaceRepository,_stateOfPlaceRepository,
                    _carRepository,_context), 
                ReservationDto.GetArrivingReservations(_reservationRepository,_parkingPlaceRepository), 
                ReservationDto.GetNoShowReservations(_reservationRepository, _parkingPlaceRepository), 
                _parkingPlaceRepository.GetNamesOfFreeParkingPlaces()));
        }

        public ActionResult CheckIn(int pPlaceId, int reservationId)
        {
            var reservation = _reservationRepository.GetReservation(reservationId);

            if (reservation.Arrival.Date != DateTime.Today.Date 
                && reservation.StateOfReservationId != StateOfReservation.TemporaryLeave)
                return RedirectToAction("Parking");

            if (_parkingPlaceRepository.GetParkingPlace(pPlaceId).StateOfPlaceId == StateOfPlace.Reserved)
            {
                reservation.CheckIn();
                _parkingPlaceRepository.GetParkingPlace(pPlaceId)
                    .Occupy(_stateOfPlaceRepository.GetOccupiedStateOfPlace(), reservation);

                _context.SaveChanges();
            }

            return RedirectToAction("Parking");
        }


        public ActionResult CheckOut(int pPlaceId)
        {
            var pPlace = _parkingPlaceRepository.GetParkingPlaceReservation(pPlaceId);

            pPlace.Reservation.CheckOut();
            pPlace.Release(_stateOfPlaceRepository.GetFreeStateOfPlace());

            _context.SaveChanges();

            return RedirectToAction("Parking");
        }

        public ActionResult TemporaryLeave(int pPlaceId)
        {
            var pPlace = _parkingPlaceRepository.GetParkingPlaceReservation(pPlaceId);

            pPlace.Reservation.TemporaryLeave();
            pPlace.Reserve(_stateOfPlaceRepository.GetReservedStateOfPlace(), pPlace.Reservation);

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
                    .Release(_stateOfPlaceRepository.GetFreeStateOfPlace());
            }
            _parkingPlaceRepository.GetParkingPlace(ParkingPlaceName)
                .Reserve(_stateOfPlaceRepository.GetReservedStateOfPlace(), reservation);

            _context.SaveChanges();

            return RedirectToAction("Parking");
        }
    }
}