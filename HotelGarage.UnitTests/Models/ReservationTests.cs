using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using HotelGarage.Models;

namespace HotelGarage.UnitTests
{
    [TestFixture]
    public class ReservationTests
    {
        Reservation _reservation;

        [SetUp]
        public void SetUp()
        {
            _reservation = new Reservation("stara", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1),
                true, 1, new Car { LicensePlate = "stara" });

            _reservation.StateOfReservationId = StateOfReservation.Reserved;
        }

        [Test]
        public void CheckOut_ReservationIsInhouse_ResDepSetPplaceIdZeroStateOfResDeparted()
        {
            _reservation.StateOfReservationId = StateOfReservation.Inhouse;

            _reservation.CheckOut();

            Assert.That(_reservation.Departure.ToShortDateString(), Is.EqualTo(DateTime.Now.ToShortDateString()));
            Assert.That(_reservation.Departure.ToShortTimeString(), Is.EqualTo(DateTime.Now.ToShortTimeString()));
            Assert.That(_reservation.ParkingPlaceId, Is.EqualTo(0));
            Assert.That(_reservation.StateOfReservationId, Is.EqualTo(StateOfReservation.Departed));
        }

        [Test]
        public void CheckIn_Reservation_StateOfResIdIsInhouseAndArrivalIsNow()
        {
            _reservation.CheckIn();

            Assert.That(_reservation.StateOfReservationId, Is.EqualTo(StateOfReservation.Inhouse));
            Assert.That(_reservation.Arrival.ToShortDateString(), Is.EqualTo(DateTime.Now.ToShortDateString()));
            Assert.That(_reservation.Arrival.ToShortTimeString(), Is.EqualTo(DateTime.Now.ToShortTimeString()));
        }

        [Test]
        public void Update_ReservationCar_ResUpdatedAndCarAssigned()
        {
            _reservation.StateOfReservationId = StateOfReservation.Departed;

            var car = new Car() { LicensePlate = "nova" };
            var reservation = new Reservation("nova", DateTime.Now, DateTime.Now, false, 5, car);
            reservation.StateOfReservationId = StateOfReservation.Inhouse;

            _reservation.Update(reservation, car);

            Assert.That(_reservation.Arrival.ToShortDateString(), Is.EqualTo(DateTime.Now.ToShortDateString()));
            Assert.That(_reservation.Arrival.ToShortTimeString(), Is.EqualTo(DateTime.Now.ToShortTimeString()));
            Assert.That(_reservation.Departure.ToShortDateString(), Is.EqualTo(DateTime.Now.ToShortDateString()));
            Assert.That(_reservation.Departure.ToShortTimeString(), Is.EqualTo(DateTime.Now.ToShortTimeString()));
            Assert.That(_reservation.IsRegistered, Is.EqualTo(false));
            Assert.That(_reservation.LicensePlate, Is.EqualTo("nova"));
            Assert.That(_reservation.ParkingPlaceId, Is.EqualTo(5));
            Assert.That(_reservation.Car, Is.EqualTo(car));
            Assert.That(_reservation.StateOfReservationId, Is.EqualTo(StateOfReservation.Inhouse));
        }

        [Test]
        public void Cancel_ResHasFreeParkingPlace_SetReservationToCancell()
        {
            ParkingPlace parkingPlace = null;

            _reservation.Cancel(parkingPlace, new StateOfPlace() { Id = 1, Name = "Volno" });

            Assert.That(parkingPlace, Is.EqualTo(null));
            Assert.That(_reservation.StateOfReservationId, Is.EqualTo(4));
        }

        [Test]
        public void Cancel_ResHasOccupiedParkingPlace_ReleaseParkingPlaceFromReservationAndSetResToCancell()
        {
            ParkingPlace parkingPlace = new ParkingPlace() { Reservation = _reservation,};

            _reservation.Cancel(parkingPlace, new StateOfPlace() { Id = 1, Name = "Volno" });

            Assert.That(parkingPlace.Reservation, Is.EqualTo(null));
            Assert.That(_reservation.StateOfReservationId, Is.EqualTo(4));
        }
    }
}
