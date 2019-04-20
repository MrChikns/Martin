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
        private Mock<ICarRepository> _mockCarRepository;
        private Mock<IStateOfPlaceRepository> _mockStatesOfPlacesRepository;
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

            _car = new Car();

            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockReservationRepository.Setup(r => r.GetReservation(_existing)).Returns(_reservation);
            _mockReservationRepository.Setup(r => r.GetReservationCar(_existing)).Returns(_reservation);

            _mockParkingPlaceRepository = new Mock<IParkingPlaceRepository>();
            _mockParkingPlaceRepository.Setup(p => p.GetParkingPlace(_existing)).Returns(_parkingPlace);
            _mockParkingPlaceRepository.Setup(p => p.GetParkingPlaceStateOfPlace(_reservation)).Returns(_parkingPlace);

            _mockCarRepository = new Mock<ICarRepository>();
            _mockCarRepository.Setup(c => c.GetCar(_reservation)).Returns(_car);

            _mockStatesOfPlacesRepository = new Mock<IStateOfPlaceRepository>();
            _mockStatesOfPlacesRepository.Setup(s => s.GetFreeStateOfPlace()).Returns(new StateOfPlace());
            _mockStatesOfPlacesRepository.Setup(s => s.GetReservedStateOfPlace()).Returns(new StateOfPlace());

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.SetupGet(u => u.Reservations).Returns(_mockReservationRepository.Object);
            _mockUnitOfWork.SetupGet(u => u.ParkingPlaces).Returns(_mockParkingPlaceRepository.Object);
            _mockUnitOfWork.SetupGet(u => u.Cars).Returns(_mockCarRepository.Object);
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
        public void Update_ReservationIdExists_ReturnView()
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
        public void Delete_ReservationIdExists_ReturnRedirectToActionResult()
        {
            var result = (RedirectToRouteResult)_controller.Delete(_existing);

            Assert.AreEqual("Parking",result.RouteValues["action"]);
            Assert.AreEqual("Parking", result.RouteValues["controller"]);
        }

        [Test]
        public void Save_ViewModelIsValid_ReturnRedirectToActionResult()
        {
            var result = (RedirectToRouteResult)_controller.Save(_reservation);

            Assert.AreEqual("Parking", result.RouteValues["action"]);
            Assert.AreEqual("Parking", result.RouteValues["controller"]);
        }

        [Test]
        public void Save_ViewModelIsInValid_ReturnView()
        {
            _controller.ModelState.AddModelError("","");
            var result = _controller.Save(_reservation) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName == "Form");
        }
    }
}
