using HotelGarage.Core.Models;
using NUnit.Framework;
using System;

namespace HotelGarage.UnitTests.Models
{
    [TestFixture]
    public class ReservationTests
    {
        Reservation _reservation;
        string _staraSPZ;

        [SetUp]
        public void SetUp()
        {
            _staraSPZ = "staraSPZ";
            _reservation = new Reservation()
            {
                LicensePlate = _staraSPZ,
                Arrival = DateTime.Now.AddDays(1),
                Departure = DateTime.Now.AddDays(1),
                IsRegistered = true,
                ParkingPlaceId = 1,
                Car = new Car { LicensePlate = _staraSPZ },
                StateOfReservationId = StateOfReservation.Reserved
            };
        }   

        [Test]
        public void CheckOut_ReservationIsNotInhouse_ThrowsArgumentOutOfRangeException()
        {
            _reservation.StateOfReservationId = StateOfReservation.Cancelled;

            Assert.That(() => _reservation.CheckOut(), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CheckIn_ReservationWithWrongState_ThrowsException()
        {
            _reservation.StateOfReservationId = StateOfReservation.Inhouse;

            Assert.That(() => _reservation.CheckIn(), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Cancel_ResHasFreeParkingPlace_SetReservationToCancell()
        {
            _reservation.Cancel(null, new StateOfPlace() { Id = 1, Name = Helpers.Constants.FreeStateOfPlaceLabel });
            
            Assert.That(_reservation.StateOfReservationId, Is.EqualTo(4));
        }

        [Test]
        public void Cancel_ResHasOccupiedParkingPlace_ReleaseParkingPlaceFromReservationAndSetResToCancell()
        {
            var parkingPlace = new ParkingPlace();
            parkingPlace.AssignReservation(_reservation);
            _reservation.Cancel(parkingPlace, new StateOfPlace() { Id = 1, Name = Helpers.Constants.FreeStateOfPlaceLabel });

            Assert.That(parkingPlace.Reservation, Is.EqualTo(null));
            Assert.That(_reservation.StateOfReservationId, Is.EqualTo(4));
        }
    }
}
