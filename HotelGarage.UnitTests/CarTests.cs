using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using HotelGarage.Models;

namespace HotelGarage.UnitTests
{
    [TestFixture]
    public class CarTests
    {
        Reservation _reservation;
        Car _car;

        [SetUp]
        public void SetUp()
        {
            _car = new Car("nova", "novy", "Novy", -1, -1, false, "nova");
            _reservation = new Reservation("nova", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1), false, -1, _car);
        }

        [Test]
        public void Update_Reservation_UpdatedReservation()
        {
            _car.Update(new Reservation("updated", DateTime.Now, DateTime.Now, true, 5, 
                new Car("updated", "updated", "Updated", 5, 5, true, "updated")));

            Assert.That(_car.LicensePlate, Is.EqualTo("updated"));
            Assert.That(_car.CarModel, Is.EqualTo("updated"));
            Assert.That(_car.GuestsName, Is.EqualTo("Updated"));
            Assert.That(_car.GuestRoomNumber, Is.EqualTo(5));
            Assert.That(_car.PricePerNight, Is.EqualTo(5));
            Assert.That(_car.IsEmployee, Is.EqualTo(true));
            Assert.That(_car.Note, Is.EqualTo("updated"));
        }
    }
}
