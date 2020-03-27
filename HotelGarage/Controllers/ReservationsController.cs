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
            var updatedReservation = _unitOfWork.Reservations.GetReservation(reservationId) ?? throw new ArgumentOutOfRangeException("Invalid reservation ID.");
            
            return View("Form", updatedReservation);
        }

        public ActionResult Delete(int reservationId)
        {
            var deletedReservation = _unitOfWork.Reservations.GetReservation(reservationId) ?? throw new ArgumentOutOfRangeException("Invalid reservation ID.");
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

            var reservation = _unitOfWork.Reservations.GetReservation(newReservationData.Id);
            var car = _unitOfWork.Cars.GetCar(newReservationData);
            car = CreateOrUpdateCar(newReservationData, car);
            reservation = CreateOrUpdateReservation(newReservationData, reservation, car);
            
            var parkingPlace = _unitOfWork.ParkingPlaces.GetParkingPlace(reservation);
            SetupReservation(reservation, parkingPlace);

            _unitOfWork.Complete();

            return RedirectToAction("Parking", "Parking");
        }

        private Reservation CreateOrUpdateReservation(Reservation viewModel, Reservation reservation, Car car)
        {
            if (viewModel.Id == 0)
            {
                reservation = viewModel;
                reservation.Car = car;
                reservation.State = ReservationState.Reserved;

                _unitOfWork.Reservations.AddReservation(reservation);
            }
            else
            {
                reservation.Update(viewModel, car);
            }

            return reservation;
        }

        private Car CreateOrUpdateCar(Reservation viewModel, Car car)
        {
            if (_unitOfWork.Cars.GetCar(viewModel) == null)
            {
                _unitOfWork.Cars.Add(new Car(viewModel.Car));
            }
            else
            {
                car.Update(viewModel);
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