using HotelGarage.Core.Models;
using HotelGarage.Helpers;
using NUnit.Framework;
using System;

namespace HotelGarage.UnitTests.Models
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
            _stateOfPlace = new StateOfPlace() { Name = Constants.OccupiedStateOfPlaceLabel };
            _reservation = new Reservation() { Id = 1};
            _parkingPlace = new ParkingPlace() { Id = 1};
            _parkingPlace.AssignStateOfPlace(_stateOfPlace);
            _parkingPlace.AssignReservation(_reservation);
        }

        [Test]
        public void GetStateOfParkingPlace_NameIsObsazenoAndReservationUnregistered_Neregistrovan()
        {
            _parkingPlace.StateOfPlace.Name = _parkingPlace.GetStateOfPlaceName();

            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.NotRegisteredStateOfPlaceLabel));
        }

        [Test]
        public void GetStateOfParkingPlaceName_NameIsObsazenoAndDepartureToday_Odjezd()
        {
            _reservation.IsRegistered = true;
            _parkingPlace.StateOfPlace.Name = _parkingPlace.GetStateOfPlaceName();

            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.DepartureStateOfPlaceLabel));
        }

        [Test]
        public void GetStateOfParkingPlaceName_NameIsVolnoAndStaffOnly_VolnoStaff()
        {
            _parkingPlace.Id = Constants.NumberOfStandardParkingPlaces + 1;
            _stateOfPlace.Name = Constants.FreeStateOfPlaceLabel;
            _parkingPlace.StateOfPlace.Name = _parkingPlace.GetStateOfPlaceName();

            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.FreeStaffStateOfPlaceLabel));
        }

        [Test]
        public void GetStateOfParkingPlaceName_NameIsRezervovano_Rezervovano()
        {
            _stateOfPlace.Name = Constants.ReservedStateOfPlaceLabel;
            _parkingPlace.StateOfPlace.Name = _parkingPlace.GetStateOfPlaceName();

            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.ReservedStateOfPlaceLabel));
        }

        [Test]
        public void Release_ParkingPlaceStateIsNotFreeAndReservationIsAssigned()
        {
            var freeStateOfPlace = new StateOfPlace() { Id = 1, Name = Constants.FreeStateOfPlaceLabel};
            _reservation.StateOfReservationId = StateOfReservation.Inhouse;
            _parkingPlace.Release(freeStateOfPlace);

            Assert.That(_reservation.ParkingPlaceId, Is.EqualTo(0));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(null));
            Assert.That(_parkingPlace.StateOfPlaceId, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Id, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.FreeStateOfPlaceLabel));
        }

        [Test]
        public void Reserve_ParkingPlaceFreeAndReservationNotAssigned()
        {
            _stateOfPlace.Id = StateOfPlace.Free;
            _stateOfPlace.Name = Constants.FreeStateOfPlaceLabel;
            _parkingPlace.AssignStateOfPlaceId(StateOfPlace.Free);

            var reservedStateOfPlace = new StateOfPlace() { Id = 3, Name = Constants.ReservedStateOfPlaceLabel };
            var reservation = new Reservation() { Id = 5};
            reservation.SetParkingPlaceId(0);
            _parkingPlace.Reserve(reservedStateOfPlace,reservation);

            Assert.That(reservation.ParkingPlaceId, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Id, Is.EqualTo(3));
            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.ReservedStateOfPlaceLabel));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(reservation));
        }

        [Test]
        public void Occupy_ParkingPlaceReserved_OccupiedByReservation()
        {
            _stateOfPlace.Id = StateOfPlace.Reserved;
            _stateOfPlace.Name = Constants.ReservedStateOfPlaceLabel;
            _parkingPlace.Reservation.SetParkingPlaceId(0);
            _parkingPlace.AssignStateOfPlaceId(StateOfPlace.Reserved);
            _parkingPlace.Id = 5;

            var occupiedStateOfPlace = new StateOfPlace() { Id = 2, Name = Constants.OccupiedStateOfPlaceLabel };
            var reservation = new Reservation() { Id = 5 };
            reservation.SetParkingPlaceId(5);
            _parkingPlace.Occupy(occupiedStateOfPlace, reservation);

            Assert.That(reservation.ParkingPlaceId, Is.EqualTo(5));
            Assert.That(_parkingPlace.StateOfPlace.Id, Is.EqualTo(2));
            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.OccupiedStateOfPlaceLabel));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(reservation));
        }

        [Test]
        public void Occupy_WrongParametersPassed_ThrowsException()
        {
            var occupied = new StateOfPlace() { Id = StateOfPlace.Occupied, Name = Constants.OccupiedStateOfPlaceLabel };
            var reservation = new Reservation() { Id = 5 };

            Assert.That(() => _parkingPlace.Occupy(occupied, reservation), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Release_ParkingPlaceNotFree_ReservationNullAndStateOfPlaceFree()
        {
            var reservation = _reservation;
            _parkingPlace.AssignStateOfPlaceId(StateOfPlace.Reserved);
            _stateOfPlace.Id = StateOfPlace.Reserved;
            _stateOfPlace.Name = Constants.ReservedStateOfPlaceLabel;

            var freeStateOfPlace = new StateOfPlace() { Id = 1, Name = Constants.FreeStateOfPlaceLabel };
            _parkingPlace.AssingnFreeParkingPlace(freeStateOfPlace,reservation);

            Assert.That(reservation.ParkingPlaceId, Is.EqualTo(0));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(null));
            Assert.That(_parkingPlace.StateOfPlaceId, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Id, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.FreeStateOfPlaceLabel));
        }

        [Test]
        public void Release_ParkingPlaceFree_ThrowsException()
        {
            var free = new StateOfPlace() { Id = StateOfPlace.Free, Name = Constants.FreeStateOfPlaceLabel};
            _parkingPlace.StateOfPlaceId = StateOfPlace.Free;

            Assert.That(() => _parkingPlace.Release(free), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void MoveInhouseReservation_ReservationInHouse_SetValuesAfterMove()
        {
            _reservation.StateOfReservationId = StateOfReservation.Inhouse;
            var inhouse = new StateOfPlace() { Id = StateOfPlace.Occupied, Name = Constants.OccupiedStateOfPlaceLabel };
            _parkingPlace.MoveInhouseReservation(inhouse, _reservation);

            Assert.That(_reservation.ParkingPlaceId, Is.EqualTo(_reservation.Id));
            Assert.That(_parkingPlace.StateOfPlaceId, Is.EqualTo(StateOfPlace.Occupied));
            Assert.That(_parkingPlace.StateOfPlace, Is.EqualTo(inhouse));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(_reservation));
        }

        [Test]
        public void MoveInhouseReservation_ReservationNotInHouse_ThrowsException()
        {
            _reservation.StateOfReservationId = StateOfReservation.Departed;
            var inhouse = new StateOfPlace() { Id = StateOfPlace.Occupied, Name = Constants.OccupiedStateOfPlaceLabel};

            Assert.That(() => _parkingPlace.MoveInhouseReservation(inhouse,_reservation), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
    }
}
