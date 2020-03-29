using HotelGarage.Core.Models;
using HotelGarage.Persistence;
using HotelGarage.Persistence.Repositories;
using HotelGarage.UnitTests.Extensions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;

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
            _mockParkingPlaces.SetSource(new[] { parkingPlace });

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

        [Test]
        public void GetParkingPlace_ReservationDoesNotExist_ReturnsNull()
        {
            var parkingPlace = new ParkingPlace() { Id = 1 };
            _mockParkingPlaces.SetSource(new[] { parkingPlace });

            var returnedParkingPlace = _repository.GetParkingPlace(new Reservation() { ParkingPlaceId = 2});

            Assert.That(returnedParkingPlace, Is.EqualTo(null));
        }

        [Test]
        public void GetParkingPlace_ReservationExists_ReturnsParkingPlace()
        {
            var parkingPlace = new ParkingPlace() { Id = 1 };
            _mockParkingPlaces.SetSource(new[] { parkingPlace });

            var returnedParkingPlace = _repository.GetParkingPlace(new Reservation() { ParkingPlaceId = 1});

            Assert.That(returnedParkingPlace.Id, Is.EqualTo(1));
        }

        [Test]
        public void GetParkingPlaceStateOfPlace_ReservationDoesNotExist_ReturnsNull()
        {
            var parkingPlace = new ParkingPlace() { Id = 1, State = ParkingPlaceState.Occupied };
            _mockParkingPlaces.SetSource(new[] { parkingPlace });

            var returnedParkingPlace = _repository.GetParkingPlace(new Reservation() { ParkingPlaceId = 2 });

            Assert.That(returnedParkingPlace, Is.EqualTo(null));
        }

        [Test]
        public void GetParkingPlaceStateOfPlace_ReservationExists_ReturnsParkingPlace()
        {
            var parkingPlace = new ParkingPlace() { Id = 1, State = ParkingPlaceState.Occupied };
            _mockParkingPlaces.SetSource(new[] { parkingPlace });

            var returnedParkingPlace = _repository.GetParkingPlace(new Reservation() { ParkingPlaceId = 1 });

            Assert.That(returnedParkingPlace.Id, Is.EqualTo(1));
            Assert.That(returnedParkingPlace.State, Is.EqualTo(ParkingPlaceState.Occupied));
        }

        [Test]
        public void GetNamesOfFreeParkingPlaces_ListIsEmpty_ReturnsNull()
        {
            _mockParkingPlaces.SetSource(new List<ParkingPlace>());

            var returnedParkingPlaces = _repository.GetFreeParkingPlaceNames();

            Assert.That(returnedParkingPlaces, Is.Empty);
        }

        [Test]
        public void GetNamesOfFreeParkingPlaces_ListIsNotEmpty_ReturnsListOfNames()
        {
            var parkingplace = new ParkingPlace() { State = ParkingPlaceState.Free, Name = "prvni" };
            var parkingplace2 = new ParkingPlace() { State = ParkingPlaceState.Free, Name = "druhe" };

            _mockParkingPlaces.SetSource(new[] { parkingplace,parkingplace2});

            var returnedParkingPlaces = _repository.GetFreeParkingPlaceNames();

            Assert.That(returnedParkingPlaces[0], Is.EqualTo("prvni"));
            Assert.That(returnedParkingPlaces[1], Is.EqualTo("druhe"));
        }

        [Test]
        public void GetParkingPlaceName_IdExists_Return()
        {
            var parkingplace = new ParkingPlace() { Id = 1, Name = "prvni" };

            _mockParkingPlaces.SetSource(new[] { parkingplace });

            var returnedParkingPlace = _repository.GetParkingPlaceName(1);

            Assert.That(returnedParkingPlace, Is.EqualTo("prvni"));
        }
    }
}
