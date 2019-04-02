using System;
using System.Security.Claims;
using System.Security.Principal;
using HotelGarage.Controllers;
using HotelGarage.Models;
using HotelGarage.Repositories;
using Moq;
using NUnit.Framework;

namespace HotelGarage.UnitTests.Controllers
{
    [TestFixture]
    public class ParkingControllerTests
    {
        //private ParkingController _parkingController;
        //private Mock<IReservationRepository> mockResRepository;

        //public ParkingControllerTests()
        //{
        //    mockResRepository = new Mock<IReservationRepository>();
        //    _parkingController = new ParkingController();
        //}

        //[Test]
        //public void CheckIn_NoReservationWithGivenId_ThrowArgumentOutOfRangeException()
        //{

        //    Assert.That(() => _parkingController.CheckIn(0, 1), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        //}
    }
}
