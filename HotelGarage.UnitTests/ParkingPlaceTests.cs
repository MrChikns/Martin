using System;
using HotelGarage.Models;
using NUnit.Framework;

namespace HotelGarage.UnitTests
{
    [TestFixture]
    public class ParkingPlaceTests
    {
        private ParkingPlace _parkingPlace;
        private StateOfPlace _stateOfPlace;
        private Reservation _reservation;


        [SetUp]
        public void SetUp()
        {
            _stateOfPlace = new StateOfPlace();
            _reservation = new Reservation();
            _parkingPlace = new ParkingPlace();
            

            _parkingPlace.StateOfPlace = _stateOfPlace;
            _parkingPlace.Reservation = _reservation;
        }

        [Test]
        public void AssignStateOfPlaceName_PPlaceNameIsObsazenoAndResUnregistered_Neregistrovan()
        {
            _stateOfPlace.Name = "Obsazeno";
            _reservation.IsRegistered = false;
            _parkingPlace.StateOfPlace.Name = ParkingPlace.AssignStateOfPlaceName(_parkingPlace);

            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo("Neregistrován!"));
        }

        [Test]
        public void AssignStateOfPlaceName_PPlaceNameIsObsazenoAndDeparturaToday_Odjezd()
        {
            _stateOfPlace.Name = "Obsazeno";
            _reservation.Departure = DateTime.Today.Date;
            _reservation.IsRegistered = true;
            _parkingPlace.StateOfPlace.Name = ParkingPlace.AssignStateOfPlaceName(_parkingPlace);

            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo("Odjezd"));
        }

        [Test]
        public void AssignStateOfPlaceName_PPlaceNameIsVolnoAndIdIsGreaterThan19_VolnoStaff()
        {
            _parkingPlace.Id = 20;
            _stateOfPlace.Name = "Volno" ;
            _parkingPlace.StateOfPlace.Name = ParkingPlace.AssignStateOfPlaceName(_parkingPlace);

            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo("Volno Staff"));
        }

        [Test]
        public void AssignStateOfPlaceName_PPlaceNameIsRezervovano_Rezervovano()
        {
            _stateOfPlace.Name = "Rezervováno";
            _parkingPlace.StateOfPlace.Name = ParkingPlace.AssignStateOfPlaceName(_parkingPlace);

            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo("Rezervováno"));
        }

        [Test]
        public void Release_ParkingPlace_ReservationNullAndStateOfPlaceFree()
        {
            _reservation.ParkingPlaceId = 1;
            _parkingPlace.StateOfPlaceId = StateOfPlace.Occupied;
            _stateOfPlace.Id = StateOfPlace.Occupied;
            _stateOfPlace.Name = "Obsazeno";
            _parkingPlace.StateOfPlace = _stateOfPlace;

            var freeStateOfPlace = new StateOfPlace() { Id = 1, Name = "Volno" };
            _parkingPlace.Release(freeStateOfPlace);

            Assert.That(_reservation.ParkingPlaceId, Is.EqualTo(0));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(null));
            Assert.That(_parkingPlace.StateOfPlaceId, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Id, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo("Volno"));
        }

        [Test]
        public void Reserve_ParkingPlace_ReservationAssignedStateOfPlaceReserved()
        {
            _stateOfPlace.Id = StateOfPlace.Free;
            _stateOfPlace.Name = "Volno";
            _parkingPlace.Id = 1;
            _parkingPlace.Reservation.ParkingPlaceId = 1;
            _parkingPlace.Reservation.Id = 1;

            var reservedStateOfPlace = new StateOfPlace() { Id = 3, Name = "Rezervováno" };
            var reservation = new Reservation() { Id = 5, ParkingPlaceId = 5 };
            _parkingPlace.Reserve(reservedStateOfPlace,reservation);

            Assert.That(reservation.ParkingPlaceId, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Id, Is.EqualTo(3));
            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo("Rezervováno"));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(reservation));
        }

        [Test]
        public void Occupy_ParkingPlace_OccupiedByReservation()
        {

            _stateOfPlace.Id = StateOfPlace.Reserved;
            _stateOfPlace.Name = "Rezervováno";
            _parkingPlace.Id = 1;
            _parkingPlace.Reservation.ParkingPlaceId = 0;
            _parkingPlace.Reservation.Id = 1;

            var occupiedStateOfPlace = new StateOfPlace() { Id = 2, Name = "Obsazeno" };
            var reservation = new Reservation() { Id = 5, ParkingPlaceId = 5 };
            _parkingPlace.Reserve(occupiedStateOfPlace, reservation);

            Assert.That(reservation.ParkingPlaceId, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Id, Is.EqualTo(2));
            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo("Obsazeno"));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(reservation));
        }

        [Test]
        public void Free_ParkingPlace_ReservationNullAndStateOfPlaceFree()
        {
            _reservation.ParkingPlaceId = 1;
            var reservation = _reservation;
            _parkingPlace.StateOfPlaceId = StateOfPlace.Reserved;
            _stateOfPlace.Id = StateOfPlace.Reserved;
            _stateOfPlace.Name = "Rezervováno";
            _parkingPlace.StateOfPlace = _stateOfPlace;

            var freeStateOfPlace = new StateOfPlace() { Id = 1, Name = "Volno" };
            _parkingPlace.Free(freeStateOfPlace,reservation);

            Assert.That(reservation.ParkingPlaceId, Is.EqualTo(0));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(null));
            Assert.That(_parkingPlace.StateOfPlaceId, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Id, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo("Volno"));
        }
    }
}
