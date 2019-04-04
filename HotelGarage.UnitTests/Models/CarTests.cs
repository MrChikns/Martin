using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using HotelGarage.Core.Models;

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
        [TestCase(2019,1,2,2019,1,1,0)] //WhenArrivalAfterDepartureInTheSameYear
        [TestCase(2019,1,1,2018,1,1,1)] //WhenArrivalAfterDepartureInDifferentYears
        public void CalculateNumberOfDays_WrongArrivalDeparturePairs_ThrowsException(
            int arrivalYear, int arrivalMonth, int arrivalDay,
            int departureYear, int departureMonth, int departureDay,
            int result)
        {
            var arrivalDate = new DateTime(arrivalYear, arrivalMonth, arrivalDay);
            var departureDate = new DateTime(departureYear, departureMonth, departureDay);

            Assert.That(() => _car.CalculateNumberOfDays(arrivalDate, departureDate),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(2019, 1, 1, 2019, 1, 2, 1)] //WhenArrivalBeforeDepartureAndInTheSameYear
        [TestCase(2019, 1, 1, 2020, 1, 1, 365)] //WhenArrivalBeforDepartureAndInDifferentYearsNoLeapYear
        [TestCase(2019, 12, 31, 2020, 3, 1, 61)] //WhenArrivalBeforDepartureAndInDifferentYearsInLeapYear
        [TestCase(2018, 1, 1, 2020, 1, 1, 730)] //WhenArrivalBeforeDepartureAndAtLeastTwoYearsApart
        [TestCase(2019, 1, 1, 2019, 1, 1, 0)] //WhenArrivalAndDepartureAreSame
        public void CalculateNumberOfDays_AcceptedArrivalDeparturePairs_ReturnsNumberOfDays(
            int arrivalYear, int arrivalMonth, int arrivalDay,
            int departureYear, int departureMonth, int departureDay,
            int result)
        {
            var arrivalDate = new DateTime(arrivalYear, arrivalMonth, arrivalDay);
            var departureDate = new DateTime(departureYear, departureMonth, departureDay);

            var calculatedNumberOfDays = _car.CalculateNumberOfDays(arrivalDate, departureDate);

            Assert.That(calculatedNumberOfDays,Is.EqualTo(result));
        }
    }
}
