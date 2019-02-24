using System;
using HotelGarage.Helpers;
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
            _parkingPlace.AssignStateOfPlace(_stateOfPlace);
            _parkingPlace.AssignReservation(_reservation);


            _parkingPlace.Id = 1;
            _parkingPlace.AssignStateOfPlaceId(StateOfPlace.Occupied);
            _parkingPlace.Reservation.Id = 1;

            _stateOfPlace.Id = StateOfPlace.Occupied;
            _stateOfPlace.Name = Constants.OccupiedStateOfPlaceConstant;

            _reservation.Id = 1;
            _reservation.SetParkingPlaceId(1);
            _reservation.IsRegistered = false;
            _reservation.SetDepartureDay(DateTime.Today.Date);

        }

        [Test]
        public void GetStateOfPlaceName_PPlaceNameIsObsazenoAndResUnregistered_Neregistrovan()
        {
            _parkingPlace.StateOfPlace.Name = _parkingPlace.GetStateOfPlaceName();

            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.NotRegisteredStateOfPlaceConstant));
        }

        [Test]
        public void GetStateOfPlaceName_PPlaceNameIsObsazenoAndDeparturaToday_Odjezd()
        {
            _reservation.IsRegistered = true;

            _parkingPlace.StateOfPlace.Name = _parkingPlace.GetStateOfPlaceName();

            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.DepartureStateOfPlaceConstant));
        }

        [Test]
        public void GetStateOfPlaceName_PPlaceNameIsVolnoAndIdIsGreaterThan19_VolnoStaff()
        {
            _parkingPlace.Id = 20; // id mista, od ktereho parkuje staff na parkovisti
            _stateOfPlace.Name = Constants.FreeStateOfPlaceConstant;

            _parkingPlace.StateOfPlace.Name = _parkingPlace.GetStateOfPlaceName();

            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.FreeStaffStateOfPlaceConstant));
        }

        [Test]
        public void GetStateOfPlaceName_PPlaceNameIsRezervovano_Rezervovano()
        {
            _stateOfPlace.Name = Constants.ReservedStateOfPlaceConstant;

            _parkingPlace.StateOfPlace.Name = _parkingPlace.GetStateOfPlaceName();

            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.ReservedStateOfPlaceConstant));
        }

        [Test]
        public void Release_ParkingPlaceStateIsNotFreeAndReservationIsAssigned_ReservationNullAndStateOfPlaceFree()
        {
            var freeStateOfPlace = new StateOfPlace() { Id = 1, Name = Constants.FreeStateOfPlaceConstant};
            _reservation.StateOfReservationId = StateOfReservation.Inhouse;
            _parkingPlace.Release(freeStateOfPlace);

            Assert.That(_reservation.ParkingPlaceId, Is.EqualTo(0));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(null));
            Assert.That(_parkingPlace.StateOfPlaceId, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Id, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.FreeStateOfPlaceConstant));
        }

        [Test]
        public void Reserve_ParkingPlaceFreeAndReservationNotAssignedToPPlace_ReservationAssignedStateOfPlaceReserved()
        {
            _stateOfPlace.Id = StateOfPlace.Free;
            _stateOfPlace.Name = Constants.FreeStateOfPlaceConstant;
            _parkingPlace.AssignStateOfPlaceId(StateOfPlace.Free);

            var reservedStateOfPlace = new StateOfPlace() { Id = 3, Name = Constants.ReservedStateOfPlaceConstant };
            var reservation = new Reservation() { Id = 5};
            reservation.SetParkingPlaceId(0);
            _parkingPlace.Reserve(reservedStateOfPlace,reservation);

            Assert.That(reservation.ParkingPlaceId, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Id, Is.EqualTo(3));
            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.ReservedStateOfPlaceConstant));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(reservation));
        }

        [Test]
        public void Occupy_ParkingPlaceReserved_OccupiedByReservation()
        {
            _stateOfPlace.Id = StateOfPlace.Reserved;
            _stateOfPlace.Name = Constants.ReservedStateOfPlaceConstant;
            _parkingPlace.Reservation.SetParkingPlaceId(0);
            _parkingPlace.AssignStateOfPlaceId(StateOfPlace.Reserved);
            _parkingPlace.Id = 5;

            var occupiedStateOfPlace = new StateOfPlace() { Id = 2, Name = Constants.OccupiedStateOfPlaceConstant };
            var reservation = new Reservation() { Id = 5 };
            reservation.SetParkingPlaceId(5);
            _parkingPlace.Occupy(occupiedStateOfPlace, reservation);

            Assert.That(reservation.ParkingPlaceId, Is.EqualTo(5));
            Assert.That(_parkingPlace.StateOfPlace.Id, Is.EqualTo(2));
            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.OccupiedStateOfPlaceConstant));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(reservation));
        }

        [Test]
        public void Free_ParkingPlaceReserved_ReservationNullAndStateOfPlaceFree()
        {
            var reservation = _reservation;
            _parkingPlace.AssignStateOfPlaceId(StateOfPlace.Reserved);
            _stateOfPlace.Id = StateOfPlace.Reserved;
            _stateOfPlace.Name = Constants.ReservedStateOfPlaceConstant;

            var freeStateOfPlace = new StateOfPlace() { Id = 1, Name = Constants.FreeStateOfPlaceConstant };
            _parkingPlace.AssingnFreeParkingPlace(freeStateOfPlace,reservation);

            Assert.That(reservation.ParkingPlaceId, Is.EqualTo(0));
            Assert.That(_parkingPlace.Reservation, Is.EqualTo(null));
            Assert.That(_parkingPlace.StateOfPlaceId, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Id, Is.EqualTo(1));
            Assert.That(_parkingPlace.StateOfPlace.Name, Is.EqualTo(Constants.FreeStateOfPlaceConstant));
        }
    }
}
