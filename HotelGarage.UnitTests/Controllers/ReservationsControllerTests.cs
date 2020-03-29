using HotelGarage.Controllers;
using HotelGarage.Core;
using HotelGarage.Core.Model;
using HotelGarage.Core.Repository;
using HotelGarage.UnitTests.Extensions;
using Moq;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace HotelGarage.UnitTests.Controllers
{
    [TestFixture]
    class ReservationsControllerTests
    {
        private ReservationController _controller;
        Mock<IUnitOfWork> _mockUnitOfWork;
        Mock<IReservationRepository> _mockReservationRepository;
        private Mock<IParkingPlaceRepository> _mockParkingPlaceRepository;
        private Mock<ICarRepository> _mockCarRepository;
        private Reservation _reservation;
        private ParkingPlace _parkingPlace;
        private Car _car;
        private int _existing = 1;
        private int _nonExisting = 2;

        [SetUp]
        public void SetUp()
        {
            _reservation = new Reservation()
            {
                Id = _existing,
                Arrival = DateTime.Today,
                State = ReservationState.Reserved,
                Car = new Car(),
                ParkingPlaceId = _existing
            };

            _parkingPlace = new ParkingPlace()
            {
                Id = _existing,
                State = ParkingPlaceState.Reserved,
                Reservation = _reservation
            };

            _car = new Car();

            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockReservationRepository.Setup(r => r.GetReservation(_existing, true)).Returns(_reservation);

            _mockParkingPlaceRepository = new Mock<IParkingPlaceRepository>();
            _mockParkingPlaceRepository.Setup(p => p.GetParkingPlace(_existing, true)).Returns(_parkingPlace);

            _mockCarRepository = new Mock<ICarRepository>();
            _mockCarRepository.Setup(c => c.GetCar(_reservation)).Returns(_car);

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.SetupGet(u => u.Reservations).Returns(_mockReservationRepository.Object);
            _mockUnitOfWork.SetupGet(u => u.ParkingPlaces).Returns(_mockParkingPlaceRepository.Object);
            _mockUnitOfWork.SetupGet(u => u.Cars).Returns(_mockCarRepository.Object);

            _controller = new ReservationController(_mockUnitOfWork.Object);
            _controller.MockCurrentUser("1", "user1@domain.com");
        }

        [Test]
        public void Update_ReservationIdDoesNotExist_ThrowArgumentOutOfRangeException()
        {
            Assert.That(()=>_controller.Update(_nonExisting), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Delete_ReservationIdDoesNotExist_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.Delete(_nonExisting), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Delete_ReservationIdExists_ReturnRedirectToActionResult()
        {
            var result = (RedirectToRouteResult)_controller.Delete(_existing);
            Assert.AreEqual("Parking",result.RouteValues["action"]);
            Assert.AreEqual("Parking", result.RouteValues["controller"]);
        }
    }
}
