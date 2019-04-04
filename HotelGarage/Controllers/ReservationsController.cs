using HotelGarage.Core;
using HotelGarage.Core.Dtos;
using HotelGarage.Core.Models;
using HotelGarage.Core.Repositories;
using HotelGarage.Persistence;
using HotelGarage.Persistence.Repositories;
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
            var updatedReservation = _unitOfWork.Reservations.GetReservationCar(resId);

            return View("Form", updatedReservation);
        }

        // zruseni rezervace
        public ActionResult Delete(int resId)
        {
            var reservationToDelete = _unitOfWork.Reservations.GetReservationCar(resId);

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


            // vytvoreni auta anebo update
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
                reservation = new Reservation()
                {
                    LicensePlate = viewModel.LicensePlate,
                    Arrival = viewModel.Arrival,
                    Departure = viewModel.Departure,
                    IsRegistered = viewModel.IsRegistered,
                    ParkingPlaceId = viewModel.ParkingPlaceId,
                    Car = car,
                    StateOfReservationId = StateOfReservation.Reserved
                };

                _unitOfWork.Reservations.AddReservation(reservation);
            }
            else
                reservation.Update(viewModel, car);

            ParkingPlaceNotAssignedAndResNotInhouseCheck(
                reservation,
                _unitOfWork.ParkingPlaces.GetParkingPlaceStateOfPlace(reservation),
                _unitOfWork.StatesOfPlaces);

            _unitOfWork.Complete();

            return RedirectToAction("Parking", "Parking");
        }

        public void ParkingPlaceNotAssignedAndResNotInhouseCheck(
            Reservation reservation, 
            ParkingPlace parkingPlace, 
            IStateOfPlaceRepository stateOfPlaceRepository)
        {
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
        }
    }
}