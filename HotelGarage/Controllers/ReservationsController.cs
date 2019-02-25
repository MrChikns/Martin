﻿using HotelGarage.Dtos;
using HotelGarage.Models;
using HotelGarage.Repositories;
using System;
using System.Web.Mvc;

namespace HotelGarage.Controllers
{
    public class ReservationsController : Controller
    {
        private ApplicationDbContext _context;
        private readonly CarRepository _carRepository;
        private readonly ReservationRepository _reservationRepository;
        private readonly ParkingPlaceRepository _parkingPlaceRepository;
        private readonly StateOfPlaceRepository _stateOfPlaceRepository;

        public ReservationsController()
        {
            _context = new ApplicationDbContext();
            _carRepository = new CarRepository(_context);
            _reservationRepository = new ReservationRepository(_context);
            _parkingPlaceRepository = new ParkingPlaceRepository(_context);
            _stateOfPlaceRepository = new StateOfPlaceRepository(_context);
        }

        // prehled rezervaci
        public ActionResult List()
        {
            return View("List", ReservationListDto.GetAllReservationDtos(_reservationRepository,
                _parkingPlaceRepository));
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
            return View("Form", _reservationRepository.GetReservationCar(resId));
        }

        // zruseni rezervace
        public ActionResult Delete(int resId)
        {
            var reservation = _reservationRepository.GetReservationCar(resId);

            reservation.Cancel(_parkingPlaceRepository.GetParkingPlace(reservation.ParkingPlaceId) 
                , _stateOfPlaceRepository.GetFreeStateOfPlace());

            _context.SaveChanges();

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
            var reservation = _reservationRepository.GetReservation(viewModel.Id);
            var car = _carRepository.GetCar(viewModel);


            // vytvoreni auta anebo update
            if (car == null)
            {
                car = new Car(viewModel.Car);
                _context.Cars.Add(car);
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

                _context.Reservations.Add(reservation);
            }
            else
                reservation.Update(viewModel, car);

            ParkingPlaceNotAssignedAndResNotInhouseCheck(
                reservation, 
                _parkingPlaceRepository.GetParkingPlaceStateOfPlace(reservation), 
                _stateOfPlaceRepository);
            
            _context.SaveChanges();

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
                    parkingPlace.Reserve(_stateOfPlaceRepository.GetReservedStateOfPlace(), reservation);
                // anebo nastaveni prazdneho park. mista
                else
                    parkingPlace.AssingnFreeParkingPlace(_stateOfPlaceRepository.GetFreeStateOfPlace(), reservation);
            }
        }
    }
}