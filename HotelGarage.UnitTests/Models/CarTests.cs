using HotelGarage.Core.Model;
using NUnit.Framework;
using System;

namespace HotelGarage.UnitTests.Models
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

        [Test]
        public void ReturnCalculatedTotalPrice_PricePerNightIsNull_NotFilledOutMessage()
        {
     
            var calculatedPricePerNight = _car.ReturnTotalPriceString(1, null);

            Assert.That(calculatedPricePerNight, Is.EqualTo(Helpers.Labels.NotFilledOut));
        }

        [Test]
        public void ReturnCalculatedTotalPrice_NumberOfDaysIsZero_PriceForOneDay()
        {
            var pricePerNight = 2;
            var calculatedPricePerNight = _car.ReturnTotalPriceString(0, pricePerNight);

            Assert.That(calculatedPricePerNight, Is.EqualTo(pricePerNight.ToString()));
        }

        [Test]
        public void ReturnCalculatedTotalPrice_NumberOfDaysIsOneOrMore_PriceMultipliedByDays()
        {
            var calculatedPricePerNight = _car.ReturnTotalPriceString(5, 1);

            Assert.That(calculatedPricePerNight, Is.EqualTo("5"));
        }

        [Test]
        [TestCase(2019,1,2,2019,1,1)] // When arrival is after departure in the same year.
        [TestCase(2019,1,1,2018,1,1)] // When arrival is after departure in different years.
        public void CalculateNumberOfDays_WrongArrivalDeparturePairs_ThrowsException(
            int arrivalYear,
            int arrivalMonth,
            int arrivalDay,
            int departureYear,
            int departureMonth,
            int departureDay
        )
        {
            var arrivalDate = new DateTime(arrivalYear, arrivalMonth, arrivalDay);
            var departureDate = new DateTime(departureYear, departureMonth, departureDay);

            Assert.That(() => _car.CalculateNumberOfDays(arrivalDate, departureDate), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(2019, 1, 1, 2019, 1, 2, 1)] //When arrival is before departure and in the same year.
        [TestCase(2019, 1, 1, 2020, 1, 1, 365)] //When arrival is before departure and in different year. Not leap year.
        [TestCase(2019, 12, 31, 2020, 3, 1, 61)] // When arrival is befor departure and in different year. In leap year.
        [TestCase(2018, 1, 1, 2020, 1, 1, 730)] // When arrival is before departure and at least two years apart.
        [TestCase(2019, 1, 1, 2019, 1, 1, 0)] // When arrival and departure are the same.
        public void CalculateNumberOfDays_AcceptedArrivalDeparturePairs_ReturnsNumberOfDays(
            int arrivalYear,
            int arrivalMonth,
            int arrivalDay,
            int departureYear,
            int departureMonth,
            int departureDay,
            int result
        )
        {
            var arrivalDate = new DateTime(arrivalYear, arrivalMonth, arrivalDay);
            var departureDate = new DateTime(departureYear, departureMonth, departureDay);
            var calculatedNumberOfDays = _car.CalculateNumberOfDays(arrivalDate, departureDate);

            Assert.That(calculatedNumberOfDays,Is.EqualTo(result));
        }
    }
}
