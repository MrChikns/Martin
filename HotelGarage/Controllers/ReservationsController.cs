using HotelGarage.Core;
using HotelGarage.Core.Dtos;
using HotelGarage.Core.Models;
using HotelGarage.Core.Repositories;
using System;
using System.Web.Mvc;

namespace HotelGarage.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // prehled rezervaci
        public ActionResult List()
        {
            var allReservations = ReservationListDto.GetAllReservationDtos(_unitOfWork);

            return View("List", allReservations);
        }

        // nova rezervace
        public ActionResult Create(int? pPlaceId)
        {

            var newReservation = new Reservation()
            {
                ParkingPlaceId = (pPlaceId != null) ? (int)pPlaceId : 0,
                StateOfReservationId = StateOfReservation.Reserved,
                Arrival = DateTime.Now,
                Departure = DateTime.Now.AddDays(1)
            };

            return View("Form", newReservation);
        }

        // update rezervace
        public ActionResult Update(int resId)
        {
            var updatedReservation = _unitOfWork.Reservations.GetReservationCar(resId)??
                throw new ArgumentOutOfRangeException("Reservation ID does not exist");

            return View("Form", updatedReservation);
        }

        // zruseni rezervace
        public ActionResult Delete(int resId)
        {
            var reservationToDelete = _unitOfWork.Reservations.GetReservationCar(resId)??
                throw new ArgumentOutOfRangeException("Reservation ID does not exist");

            var reservationsParkingPlace = _unitOfWork.ParkingPlaces.GetParkingPlace(reservationToDelete.ParkingPlaceId);
            var freeStateOfPlace = _unitOfWork.StatesOfPlaces.GetFreeStateOfPlace();

            reservationToDelete.Cancel(reservationsParkingPlace, freeStateOfPlace);

            _unitOfWork.Complete();

            return RedirectToAction("Parking", "Parking");
        }

        // ulozeni nove nebo upravovane rezervace a auta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Reservation viewModel)
        {
            if (!ModelState.IsValid)
                return View("Form", viewModel);

            // prirazeni auta a rezervace pokud jiz existuji a nastaveni jejich atributu
            var reservation = _unitOfWork.Reservations.GetReservation(viewModel.Id);
            var car = _unitOfWork.Cars.GetCar(viewModel);

            // vytvoreni auta anebo update stavajiciho
            if (_unitOfWork.Cars.GetCar(viewModel) == null)
            {
                car = new Car(viewModel.Car);
                _unitOfWork.Cars.Add(car);
            }
            else
                car.Update(viewModel);

            //vytvoreni rezervace anebo update
            if (viewModel.Id == 0)
            {
                reservation = viewModel;
                reservation.Car = car;
                reservation.StateOfReservationId = StateOfReservation.Reserved;

                _unitOfWork.Reservations.AddReservation(reservation);
            }
            else
                reservation.Update(viewModel, car);

            var parkingPlace = _unitOfWork.ParkingPlaces.GetParkingPlaceStateOfPlace(reservation);

            if (reservation.ParkingPlaceId != 0 && reservation.StateOfReservationId != StateOfReservation.Inhouse)
            {
                // prirazeni k mistu rezervace a nastaveni mista na rezervovano
                if (reservation.StateOfReservationId == StateOfReservation.Reserved
                    && reservation.Arrival.DayOfYear == DateTime.Today.DayOfYear)
                    parkingPlace.Reserve(_unitOfWork.StatesOfPlaces.GetReservedStateOfPlace(), reservation);
                // anebo nastaveni prazdneho park. mista
                else
                    parkingPlace.AssingnFreeParkingPlace(_unitOfWork.StatesOfPlaces.GetFreeStateOfPlace(), reservation);
            }

            _unitOfWork.Complete();

            return RedirectToAction("Parking", "Parking");
        }
    }
}