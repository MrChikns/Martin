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
        Car _car;

        [SetUp]
        public void SetUp()
        {
            _car = new Car();
        }

        //ReturnCalculatedTotalPriceString
        [Test]
        public void ReturnCalculatedTotalPriceString_PricePerNightIsNull_ReturnsNotFilledOutMessage()
        {
     
            var calculatedPricePerNight = _car.ReturnCalculatedTotalPriceString(1, null);

            Assert.That(calculatedPricePerNight, Is.EqualTo(Helpers.Constants.NotFilledOutMessageConstant));
        }

        [Test]
        public void ReturnCalculatedTotalPriceString_NumberOfDaysIsZero_ReturnsAPriceForOneDay()
        {
            var pricePerNight = 2;
            var calculatedPricePerNight = _car.ReturnCalculatedTotalPriceString(0, pricePerNight);

            Assert.That(calculatedPricePerNight, Is.EqualTo(pricePerNight.ToString()));
        }

        [Test]
        public void ReturnCalculatedTotalPriceString_NumberOfDaysIsOneOrMore_ReturnsAPriceMultipliedByDays()
        {
            var calculatedPricePerNight = _car.ReturnCalculatedTotalPriceString(5, 1);

            Assert.That(calculatedPricePerNight, Is.EqualTo("5"));
        }

        // CalculateNumberOfDays
        [Test]
        public void CalculateNumberOfDays_WhenArrivalAfterDepartureInTheSameYear_ThrowsException()
        {
            var arrivalDate = new DateTime(2019, 1, 2);
            var departureDate = new DateTime(2019, 1, 1);

            Assert.That(() => _car.CalculateNumberOfDays(arrivalDate, departureDate), 
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CalculateNumberOfDays_WhenArrivalAfterDepartureInDifferentYears_ThrowsException()
        {
            var arrivalDate = new DateTime(2019, 1, 1);
            var departureDate = new DateTime(2018, 1, 1);

            Assert.That(() => _car.CalculateNumberOfDays(arrivalDate, departureDate),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CalculateNumberOfDays_WhenArrivalBeforeDepartureAndInTheSameYear_ReturnsNumberOfDays()
        {
            var arrivalDate = new DateTime(2019, 1, 1);
            var departureDate = new DateTime(2019, 1, 2);

            var calculatedNumberOfDays = _car.CalculateNumberOfDays(arrivalDate,departureDate);

            Assert.That(calculatedNumberOfDays, Is.EqualTo(1));
        }

        [Test]
        public void CalculateNumberOfDays_WhenArrivalBeforDepartureAndInDifferentYearsNoLeapYear_ReturnsNumberOfDays()
        {
            var arrivalDate = new DateTime(2019, 1, 1);
            var departureDate = new DateTime(2020, 1, 1);

            var calculatedNumberOfDays = _car.CalculateNumberOfDays(arrivalDate, departureDate);

            Assert.That(calculatedNumberOfDays, Is.EqualTo(365));
        }

        [Test]
        public void CalculateNumberOfDays_WhenArrivalBeforDepartureAndInDifferentYearsInLeapYear_ReturnsNumberOfDays()
        {
            var arrivalDate = new DateTime(2019, 12, 31);
            var departureDate = new DateTime(2020, 3, 1); // 1 + 31 + 29

            var calculatedNumberOfDays = _car.CalculateNumberOfDays(arrivalDate, departureDate);

            Assert.That(calculatedNumberOfDays, Is.EqualTo(61));
        }

        [Test]
        public void CalculateNumberOfDays_WhenArrivalBeforeDepartureAndAtLeastTwoYearsApart_ReturnsNumberOfDays()
        {
            var arrivalDate = new DateTime(2018, 1, 1);
            var departureDate = new DateTime(2020, 1, 1);

            var calculatedNumberOfDays = _car.CalculateNumberOfDays(arrivalDate, departureDate);

            Assert.That(calculatedNumberOfDays, Is.EqualTo(730));
        }

        [Test]
        public void CalculateNumberOfDays_WhenArrivalAndDepartureAreSame_ReturnsNumberOfDays()
        {
            var arrivalDate = new DateTime(2019, 1, 1);
            var departureDate = new DateTime(2019, 1, 1);

            var calculatedNumberOfDays = _car.CalculateNumberOfDays(arrivalDate, departureDate);

            Assert.That(calculatedNumberOfDays, Is.EqualTo(0));
        }
    }
}
