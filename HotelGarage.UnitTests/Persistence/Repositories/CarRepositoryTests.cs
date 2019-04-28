using HotelGarage.Core.Models;
using HotelGarage.Persistence;
using HotelGarage.Persistence.Repositories;
using HotelGarage.UnitTests.Extensions;
using Moq;
using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace HotelGarage.UnitTests.Persistence.Repositories
{

    [TestFixture]
    class CarRepositoryTests
    {
        private CarRepository _repository;
        private Mock<DbSet<Car>> _mockCars;

        [SetUp]
        public void TestInitialize()
        {
            _mockCars = new Mock<DbSet<Car>>();

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Cars).Returns(() => _mockCars.Object);

            _repository = new CarRepository(mockContext.Object);
        }

        [Test]
        public void GetCar_ReservationExists_ReturnCar()
        {
            var car = new Car() { LicensePlate = "aa"};
            var reservation = new Reservation() { Car = car};

            _mockCars.SetSource(new[] { car });

            var returnedCar = _repository.GetCar(reservation);

            Assert.That(returnedCar, Is.EqualTo(car));
        }

        [Test]
        public void GetCar_ReservationDoesNotExist_ReturnNull()
        {
            var car = new Car() { LicensePlate = "aa" };
            var reservation = new Reservation() { Car = car };

            _mockCars.SetSource(new[] { new Car() { LicensePlate = "bb"} });

            var returnedCar = _repository.GetCar(reservation);

            Assert.That(returnedCar, Is.EqualTo(null));
        }

    }
}
