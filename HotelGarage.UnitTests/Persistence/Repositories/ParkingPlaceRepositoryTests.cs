using HotelGarage.Core.Models;
using HotelGarage.Persistence;
using HotelGarage.Persistence.Repositories;
using HotelGarage.UnitTests.Extensions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGarage.UnitTests.Persistence.Repositories
{
    [TestFixture]
    class ParkingPlaceRepositoryTests
    {
        private Mock<DbSet<ParkingPlace>> _mockParkingPlaces;
        private ParkingPlaceRepository _repository;

        [SetUp]
        public void TestInitialize()
        {
            _mockParkingPlaces = new Mock<DbSet<ParkingPlace>>();

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.ParkingPlaces).Returns(() => _mockParkingPlaces.Object);

            _repository = new ParkingPlaceRepository(mockContext.Object);
        }

        [Test]
        public void GetParkingPlace_NameDoesNotExist_ReturnsNull()
        {
            var parkingPlace = new ParkingPlace() { Name = "aa"};
            _mockParkingPlaces.SetSource(new[] { parkingPlace});

            var returnedParkingPlace = _repository.GetParkingPlace("bb");

            Assert.That(returnedParkingPlace, Is.EqualTo(null));
        }

        [Test]
        public void GetParkingPlace_NameExists_ReturnsParkingPlace()
        {
            var parkingPlace = new ParkingPlace() { Name = "aa" };
            _mockParkingPlaces.SetSource(new[] { parkingPlace });

            var returnedParkingPlace = _repository.GetParkingPlace("aa");

            Assert.That(returnedParkingPlace.Name, Is.EqualTo("aa"));
        }
    }
}
