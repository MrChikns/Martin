using HotelGarage.Core.Models;
using HotelGarage.Persistence;
using HotelGarage.Persistence.Repositories;
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

        [SetUp]
        public void TestInitialize()
        {
            var mockCars = new Mock<DbSet<Car>>();

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Cars).Returns(mockCars.Object);

            _repository = new CarRepository(mockContext.Object);
        }

        [Test]
        public void GetCar_ReservationExists_ReturnCar()
        {
            var car = new Car() { };
        }

    }
}
