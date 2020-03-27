using HotelGarage.Core;
using HotelGarage.Core.Models;
using HotelGarage.Core.ViewModels;
using System;
using System.Web.Mvc;

namespace HotelGarage.Controllers
{

    public class ParkingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
            
        public ParkingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        public ActionResult Parking()
        {
            return View(new ParkingViewModel(_unitOfWork));
        }

        public ActionResult CheckIn(int parkingPlaceId, int reservationId)
        {
            var reservation = _unitOfWork.Reservations.GetReservationCar(reservationId) ?? throw new ArgumentOutOfRangeException("Invalid reservation Id.");
            var parkingPlace = _unitOfWork.ParkingPlaces.GetParkingPlace(parkingPlaceId) ?? throw new ArgumentOutOfRangeException("Invalid parking place Id.");

            if (parkingPlace.State != StateOfPlaceEnum.Reserved)
            {
                throw new ArgumentException("Invalid parking place state. Only reserved parking places can be checked in.");
            }
            if (reservation.Arrival.Date != DateTime.Today.Date && reservation.State != StateOfReservationEnum.TemporaryLeave)
            {
                throw new ArgumentException("Reservation not arriving today has to be in temporary leave state to check in again.");
            }

            reservation.CheckIn();
            parkingPlace.Occupy(reservation);

            _unitOfWork.Complete();
            
            return RedirectToAction("Parking");
        }

        public ActionResult CheckOut(int parkingPlaceId)
        {
            var parkingPlace = _unitOfWork.ParkingPlaces.GetParkingPlaceReservationCar(parkingPlaceId) ?? throw new ArgumentOutOfRangeException("Invalid parking place ID."); 

            parkingPlace.Reservation.CheckOut();
            parkingPlace.Release();

            _unitOfWork.Complete();

            return RedirectToAction("Parking");
        }

        public ActionResult TemporaryLeave(int parkingPlaceId)
        {
            var parkingPlace = _unitOfWork.ParkingPlaces.GetParkingPlaceReservationCar(parkingPlaceId) ?? throw new ArgumentOutOfRangeException("Invalid parking place ID.");

            parkingPlace.Reservation.TemporaryLeave();
            parkingPlace.Reserve(parkingPlace.Reservation);

            _unitOfWork.Complete();

            return RedirectToAction("Parking");
        }

        [HttpPost]
        public ActionResult Reserve(string parkingPlaceName, int reservationId)
        {
            var reservation = _unitOfWork.Reservations.GetReservation(reservationId) ?? throw new ArgumentOutOfRangeException("Invalid reservation ID.");

            ReleasePreviouslyReservedPlace(reservation);
            MoveOrDirectlyReserveParkingPlace(reservation, parkingPlaceName);

            _unitOfWork.Complete();

            return RedirectToAction("Parking");
        }

        public void ReleasePreviouslyReservedPlace(Reservation reservation) {
            if (reservation.ParkingPlaceId != 0)
            {
                var parkingPlace = _unitOfWork.ParkingPlaces.GetParkingPlace(reservation);
                parkingPlace.Release();
            }
        }

        public void MoveOrDirectlyReserveParkingPlace(Reservation reservation, string ParkingPlaceName) {
            if (reservation.State == StateOfReservationEnum.Inhouse)
            {
                _unitOfWork.ParkingPlaces.GetParkingPlace(ParkingPlaceName).MoveInhouseReservation(reservation);
            }
            else
            {
                _unitOfWork.ParkingPlaces.GetParkingPlace(ParkingPlaceName).Reserve(reservation);
            }
        }
    }
}