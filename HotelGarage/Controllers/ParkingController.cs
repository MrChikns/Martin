using HotelGarage.Dtos;
using HotelGarage.Models;
using HotelGarage.Repositories;
using HotelGarage.ViewModels;
using System;
using System.Web.Mvc;

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
            return View(new ParkingViewModel(_parkingPlaceRepository, _stateOfPlaceRepository,
                _reservationRepository, _carRepository, _context));
        }

        public ActionResult CheckIn(int pPlaceId, int reservationId)
        {
            var reservation = _reservationRepository.GetReservationCar(reservationId);

            CheckEligibilityToCheckIn(reservation);
            CheckInAndSaveContext(pPlaceId, reservation);

            return RedirectToAction("Parking");
        }


        public ActionResult CheckOut(int pPlaceId)
        {
            var pPlace = _parkingPlaceRepository.GetParkingPlaceReservationCar(pPlaceId);

            pPlace.Reservation.CheckOut();
            pPlace.Release(_stateOfPlaceRepository.GetFreeStateOfPlace());

            _context.SaveChanges();

            return RedirectToAction("Parking");
        }

        public ActionResult TemporaryLeave(int pPlaceId)
        {
            var pPlace = _parkingPlaceRepository.GetParkingPlaceReservationCar(pPlaceId);

            pPlace.Reservation.TemporaryLeave();
            pPlace.Reserve(_stateOfPlaceRepository.GetReservedStateOfPlace(), pPlace.Reservation);

            _context.SaveChanges();

            return RedirectToAction("Parking");
        }

        [HttpPost]
        public ActionResult Reserve(string ParkingPlaceName, int ReservationId)
        {
            var reservation = _reservationRepository.GetReservation(ReservationId);

            ReleasePreviouslyReservedPlace(reservation);
            MoveOrDirectlyReserveParkingPlace(reservation, ParkingPlaceName);

            _context.SaveChanges();

            return RedirectToAction("Parking");
        }

        public RedirectToRouteResult CheckEligibilityToCheckIn(Reservation reservation)
        {
            if (reservation.Arrival.Date != DateTime.Today.Date
                && reservation.StateOfReservationId != StateOfReservation.TemporaryLeave)
                return RedirectToAction("Parking");

            return null;
        }

        public void CheckInAndSaveContext(int pPlaceId, Reservation reservation)
        {
            if (_parkingPlaceRepository.GetParkingPlace(pPlaceId).StateOfPlaceId == StateOfPlace.Reserved)
            {
                reservation.CheckIn();
                _parkingPlaceRepository.GetParkingPlace(pPlaceId)
                    .Occupy(_stateOfPlaceRepository.GetOccupiedStateOfPlace(), reservation);

                _context.SaveChanges();
            }
        }

        public void ReleasePreviouslyReservedPlace(Reservation reservation) {
            if (reservation.ParkingPlaceId != 0)
            {
                _parkingPlaceRepository.GetParkingPlace(reservation)
                    .Release(_stateOfPlaceRepository.GetFreeStateOfPlace());
            }
        }

        public void MoveOrDirectlyReserveParkingPlace(Reservation reservation, string ParkingPlaceName) {
            if (reservation.StateOfReservationId == StateOfReservation.Inhouse)
            {
                _parkingPlaceRepository.GetParkingPlace(ParkingPlaceName)
                .MoveInhouseReservation(_stateOfPlaceRepository.GetOccupiedStateOfPlace(), reservation);
            }
            else
            {
                _parkingPlaceRepository.GetParkingPlace(ParkingPlaceName)
                    .Reserve(_stateOfPlaceRepository.GetReservedStateOfPlace(), reservation);
            }
        }
}
}