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

        public ParkingControllerTests()
        {
            _mockReservationRepository = new Mock<IReservationRepository>();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.SetupGet(u => u.Reservations).Returns(_mockReservationRepository.Object);

            _controller = new ParkingController(_mockUnitOfWork.Object);
            _controller.MockCurrentUser("1", "user1@domain.com");
        }

        [Test]
        public void CheckIn_NoReservationWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.CheckIn(1, 1),Throws.Exception.TypeOf<ArgumentOutOfRangeException>()); 
        }

    }
}
