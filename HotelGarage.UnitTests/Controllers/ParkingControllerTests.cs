using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using HotelGarage.Controllers;
using HotelGarage.Core;
using HotelGarage.Core.Models;
using HotelGarage.Core.Repositories;
using HotelGarage.UnitTests.Extensions;
using Moq;
using NUnit.Framework;

namespace HotelGarage.UnitTests.Controllers
{
    [TestFixture]
    public class ParkingControllerTests
    {
        private ParkingController _controller;
        Mock<IUnitOfWork> _mockUnitOfWork;
        Mock<IReservationRepository> _mockReservationRepository;
        private Mock<IParkingPlaceRepository> _mockParkingPlaceRepository;

        [SetUp]
        public void SetUp()
        {
            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockParkingPlaceRepository = new Mock<IParkingPlaceRepository>();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.SetupGet(u => u.Reservations).Returns(_mockReservationRepository.Object);
            _mockUnitOfWork.SetupGet(u => u.ParkingPlaces).Returns(_mockParkingPlaceRepository.Object);

            _controller = new ParkingController(_mockUnitOfWork.Object);
            _controller.MockCurrentUser("1", "user1@domain.com");
        }

        [Test]
        public void CheckIn_NoReservationWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.CheckIn(1, 1),Throws.Exception.TypeOf<ArgumentOutOfRangeException>()); 
        }

        [Test]
        public void CheckOut_NoParkingPlaceWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.CheckOut(1), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void TemporaryLeave_NoParkingPlaceWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.TemporaryLeave(1), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Reserve_NoReservationWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.Reserve("", 1), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CheckInAndSaveContext_OutOfRangeParkingPlaceIdPassed_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.CheckInAndSaveContext(1, new Reservation()), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CheckInAndSaveContext_ParkingPlaceIdOfWrongStatePassed_ThrowArgumentOutOfRangeException()
        {
            _mockParkingPlaceRepository.Setup(p => p.GetParkingPlace(1)).Returns(new ParkingPlace() { StateOfPlaceId = StateOfPlace.Occupied});

            Assert.That(() => _controller.CheckInAndSaveContext(1, new Reservation()), Throws.Exception.TypeOf<ArgumentException>());
        }
    }
}
