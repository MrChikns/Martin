using HotelGarage.Core;
using HotelGarage.Core.Dtos;
using HotelGarage.Core.Models;
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

        public ActionResult List()
        {
            return View("List", ReservationListDto.GetAllReservationDtos(_unitOfWork));
        }

        public ActionResult Create(int? parkingPlaceId)
        {
            return View("Form", new Reservation()
            {
                ParkingPlaceId = parkingPlaceId ?? 0,
                State = ReservationState.Reserved,
                Arrival = DateTime.Now,
                Departure = DateTime.Now.AddDays(1)
            });
        }

        public ActionResult Update(int reservationId)
        {
            var updatedReservation = _unitOfWork.Reservations.GetReservation(reservationId, includeCar: true) ?? throw new ArgumentOutOfRangeException("Invalid reservation ID.");
            
            return View("Form", updatedReservation);
        }

        public ActionResult Delete(int reservationId)
        {
            var deletedReservation = _unitOfWork.Reservations.GetReservation(reservationId, includeCar: true) ?? throw new ArgumentOutOfRangeException("Invalid reservation ID.");
            var reservationParkingPlace = _unitOfWork.ParkingPlaces.GetParkingPlace(deletedReservation.ParkingPlaceId);
            deletedReservation.Cancel(reservationParkingPlace);

            _unitOfWork.Complete();

            return RedirectToAction("Parking", "Parking");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Reservation newReservationData)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", newReservationData);
            }

            var car = CreateOrUpdateCar(newReservationData);
            var reservation = CreateOrUpdateReservation(newReservationData, car);
            var parkingPlace = _unitOfWork.ParkingPlaces.GetParkingPlace(reservation);
            SetupReservation(reservation, parkingPlace);

            _unitOfWork.Complete();

            return RedirectToAction("Parking", "Parking");
        }

        private Reservation CreateOrUpdateReservation(Reservation newReservation, Car car)
        {
            Reservation reservation;

            if (newReservation.Id == 0)
            {
                newReservation.Car = car;
                newReservation.State = ReservationState.Reserved;
                _unitOfWork.Reservations.AddReservation(newReservation);

                reservation = newReservation;
            }
            else
            {
                reservation = _unitOfWork.Reservations.GetReservation(newReservation.Id, includeCar: true);
                reservation.Update(newReservation, car);
            }

            return reservation;
        }

        private Car CreateOrUpdateCar(Reservation reservation)
        {
            var car = _unitOfWork.Cars.GetCar(reservation);
            
            if (car == null)
            {
                _unitOfWork.Cars.Add(reservation.Car);
                car = reservation.Car;
            }
            else
            {
                car.Update(reservation.Car);
            }

            return car;
        }

        private void SetupReservation(Reservation reservation, ParkingPlace parkingPlace)
        {
            if (reservation.ParkingPlaceId != 0 && reservation.State != ReservationState.Inhouse)
            {
                if (reservation.State == ReservationState.Reserved && reservation.Arrival.Date == DateTime.Today.Date)
                {
                    parkingPlace.Reserve(reservation);
                }
                else
                {
                    parkingPlace.AssingnFreeParkingPlace(reservation);
                }
            }
        }
    }
}