using HotelGarage.Dtos;
using HotelGarage.Models;
using HotelGarage.Persistence;
using HotelGarage.Repositories;
using System;
using System.Web.Mvc;

namespace HotelGarage.Controllers
{
    public class ReservationsController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public ReservationsController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        // prehled rezervaci
        public ActionResult List()
        {
            return View("List", ReservationListDto.GetAllReservationDtos(_unitOfWork.Reservations,
                _unitOfWork.ParkingPlaces));
        }

        // nova rezervace
        public ActionResult Create(int? pPlaceId)
        {
            return View("Form", 
                new Reservation()
                {
                    ParkingPlaceId = (pPlaceId != null) ? (int)pPlaceId : 0,
                    StateOfReservationId = StateOfReservation.Reserved,
                    Arrival = DateTime.Now,
                    Departure = DateTime.Now.AddDays(1)
                });
        }

        // update rezervace
        public ActionResult Update(int resId)
        {
            return View("Form", _unitOfWork.Reservations.GetReservationCar(resId));
        }

        // zruseni rezervace
        public ActionResult Delete(int resId)
        {
            var reservation = _unitOfWork.Reservations.GetReservationCar(resId);

            reservation.Cancel(_unitOfWork.ParkingPlaces.GetParkingPlace(reservation.ParkingPlaceId) 
                , _unitOfWork.StatesOfPlaces.GetFreeStateOfPlace());

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
            if (car == null)
            {
                car = new Car(viewModel.Car);
                _unitOfWork.Cars.AddCar(car);
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
            StateOfPlaceRepository stateOfPlaceRepository)
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