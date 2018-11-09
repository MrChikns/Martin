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
            _stateOfPlace.Id = 2;
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
    }
}
