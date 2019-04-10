using HotelGarage.Controllers;
using HotelGarage.Core;
using HotelGarage.Core.Models;
using HotelGarage.Core.Repositories;
using HotelGarage.UnitTests.Extensions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace HotelGarage.UnitTests.Controllers
{
    [TestFixture]
    class ReservationsControllerTests
    {
        private ReservationsController _controller;
        Mock<IUnitOfWork> _mockUnitOfWork;
        Mock<IReservationRepository> _mockReservationRepository;
        private Mock<IParkingPlaceRepository> _mockParkingPlaceRepository;
        private Mock<IStateOfPlaceRepository> _mockStatesOfPlacesRepository;
        private Reservation _reservation;
        private ParkingPlace _parkingPlace;
        private int _existing = 1;
        private int _nonExisting = 2;

        [SetUp]
        public void SetUp()
        {
            _reservation = new Reservation()
            {
                Id = _existing,
                Arrival = DateTime.Today,
                StateOfReservationId = StateOfReservation.Reserved,
                Car = new Car(),
                ParkingPlaceId = _existing
            };

            _parkingPlace = new ParkingPlace()
            {
                Id = _existing,
                StateOfPlace =
                new StateOfPlace() { Id = StateOfPlace.Reserved },
                StateOfPlaceId = StateOfPlace.Reserved,
                Reservation = _reservation
            };

            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockReservationRepository.Setup(r => r.GetReservationCar(_existing)).Returns(_reservation);

            _mockParkingPlaceRepository = new Mock<IParkingPlaceRepository>();
            _mockParkingPlaceRepository.Setup(p => p.GetParkingPlace(_existing)).Returns(_parkingPlace);

            _mockStatesOfPlacesRepository = new Mock<IStateOfPlaceRepository>();
            _mockStatesOfPlacesRepository.Setup(s => s.GetFreeStateOfPlace()).Returns(new StateOfPlace());
            //_mockStatesOfPlacesRepository.Setup(s => s.GetOccupiedStateOfPlace()).Returns(new StateOfPlace());

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.SetupGet(u => u.Reservations).Returns(_mockReservationRepository.Object);
            _mockUnitOfWork.SetupGet(u => u.ParkingPlaces).Returns(_mockParkingPlaceRepository.Object);
            _mockUnitOfWork.SetupGet(u => u.StatesOfPlaces).Returns(_mockStatesOfPlacesRepository.Object);

            _controller = new ReservationsController(_mockUnitOfWork.Object);
            _controller.MockCurrentUser("1", "user1@domain.com");
        }

        [Test]
        public void Update_ReservationIdDoesNotExist_ThrowArgumentOutOfRangeException()
        {
            Assert.That(()=>_controller.Update(_nonExisting), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Update_ReservationIdExists_ReturnsView()
        {
            var result = _controller.Update(_existing) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName == "Form");
        }

        [Test]
        public void Delete_ReservationIdDoesNotExist_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.Delete(_nonExisting), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Delete_ReservationIdExists_ReturnsView()
        {
            var result = (RedirectToRouteResult)_controller.Delete(_existing);

            Assert.AreEqual("Parking",result.RouteValues["action"]);
            Assert.AreEqual("Parking", result.RouteValues["controller"]);
        }

    }
}
