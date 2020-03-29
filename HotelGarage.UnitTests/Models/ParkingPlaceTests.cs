using HotelGarage.Core.Model;
using NUnit.Framework;
using System;

namespace HotelGarage.UnitTests.Models
{
    [TestFixture]
    public class ParkingPlaceTests
    {
        private ParkingPlace _parkingPlace;
        private ParkingPlaceState _stateOfPlace;
        private Reservation _reservation;

        [SetUp]
        public void SetUp()
        {
            _stateOfPlace = ParkingPlaceState.Occupied;
            _reservation = new Reservation() { Id = 1};
            _parkingPlace = new ParkingPlace() { Id = 1};
            _parkingPlace.State = _stateOfPlace;
            _parkingPlace.Reservation = _reservation;
        }

        [Test]
        public void Release_ParkingPlaceStateIsNotFreeAndReservationIsAssigned()
        {
            _reservation.State = ReservationState.Inhouse;
            _parkingPlace.Release();

            Assert.That(_reservation.ParkingPlaceId, Is.EqualTo(0));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(null));
            Assert.That(_parkingPlace.State, Is.EqualTo(ParkingPlaceState.Free));
        }

        [Test]
        public void Reserve_ParkingPlaceFreeAndReservationNotAssigned()
        {
            _parkingPlace.State = ParkingPlaceState.Free;

            var reservation = new Reservation() { Id = 5};
            reservation.SetParkingPlaceId(0);
            _parkingPlace.Reserve(reservation);

            Assert.That(reservation.ParkingPlaceId, Is.EqualTo(1));
            Assert.That(_parkingPlace.State, Is.EqualTo(ParkingPlaceState.Reserved));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(reservation));
        }

        [Test]
        public void Occupy_ParkingPlaceReserved_OccupiedByReservation()
        {
            _parkingPlace.Reservation.SetParkingPlaceId(0);
            _parkingPlace.State = ParkingPlaceState.Reserved;
            _parkingPlace.Id = 5;

            var reservation = new Reservation() { Id = 5 };
            reservation.SetParkingPlaceId(5);
            _parkingPlace.Occupy(reservation);

            Assert.That(reservation.ParkingPlaceId, Is.EqualTo(5));
            Assert.That(_parkingPlace.State, Is.EqualTo(ParkingPlaceState.Occupied));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(reservation));
        }

        [Test]
        public void Occupy_WrongParametersPassed_ThrowsException()
        {
            var reservation = new Reservation() { Id = 5 };

            Assert.That(() => _parkingPlace.Occupy(reservation), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Release_ParkingPlaceNotFree_ReservationNullAndStateOfPlaceFree()
        {
            var reservation = _reservation;
            _parkingPlace.State = ParkingPlaceState.Reserved;
            _parkingPlace.AssingnFreeParkingPlace(reservation);

            Assert.That(reservation.ParkingPlaceId, Is.EqualTo(0));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(null));
            Assert.That(_parkingPlace.State, Is.EqualTo(ParkingPlaceState.Free));
        }
    }
}
