using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using HotelGarage.Controllers;
using HotelGarage.Core;
using HotelGarage.Core.Models;
using HotelGarage.Core.Repositories;
using HotelGarage.Persistence;
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
        private Mock<IStateOfPlaceRepository> _mockStatesOfPlacesRepository;
        private Reservation _reservation;
        private ParkingPlace _parkingPlace;
        private int _existing = 1;
        private int _nonExisting = 2;
        
        [SetUp]
        public void SetUp()
        {
            _reservation = new Reservation() { Id = _existing, Arrival = DateTime.Today,
                StateOfReservationId = StateOfReservation.Reserved, Car = new Car(), ParkingPlaceId = _existing };
            _parkingPlace = new ParkingPlace() { Id = _existing, StateOfPlace = 
                new StateOfPlace() { Id = StateOfPlace.Reserved}, StateOfPlaceId = StateOfPlace.Reserved };

            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockReservationRepository.Setup(r => r.GetReservationCar(_existing)).Returns(_reservation);

            _mockParkingPlaceRepository = new Mock<IParkingPlaceRepository>();
            _mockParkingPlaceRepository.Setup(p => p.GetParkingPlace(_existing)).Returns(_parkingPlace);

            _mockStatesOfPlacesRepository = new Mock<IStateOfPlaceRepository>();
            _mockStatesOfPlacesRepository.Setup(s => s.GetOccupiedStateOfPlace()).Returns(new StateOfPlace());

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.SetupGet(u => u.Reservations).Returns(_mockReservationRepository.Object);
            _mockUnitOfWork.SetupGet(u => u.ParkingPlaces).Returns(_mockParkingPlaceRepository.Object);
            _mockUnitOfWork.SetupGet(u => u.StatesOfPlaces).Returns(_mockStatesOfPlacesRepository.Object);

            _controller = new ParkingController(_mockUnitOfWork.Object);
            _controller.MockCurrentUser("1", "user1@domain.com");
        }

        [Test]
        public void CheckIn_ExistingReservationAndParkingPlace_ReturnsRedirectToActionResult()
        {
            var result = (RedirectToRouteResult)_controller.CheckIn(_existing,_existing);            

            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result.RouteValues.ContainsValue("Parking"));
        }

        [Test]
        public void CheckIn_NoReservationWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.CheckIn(_existing, _nonExisting),Throws.Exception.TypeOf<ArgumentOutOfRangeException>()); 
        }

        [Test]
        public void CheckIn_NoParkingPlaceWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.CheckIn(_nonExisting, _existing), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CheckIn_StateOfPlaceNotReserved_ThrowArgumentException()
        {
            _parkingPlace.StateOfPlaceId = StateOfPlace.Occupied;

            Assert.That(() => _controller.CheckIn(_existing, _existing), Throws.Exception.TypeOf<ArgumentException>());
        }

        [Test]
        public void CheckIn_ArrivalNotTodayAndNoTemporaryLeave_ThrowArgumentOutOfRangeException()
        {
            _reservation.Arrival = DateTime.Today.AddDays(1);

            Assert.That(() => _controller.CheckIn(_existing, _existing), Throws.Exception.TypeOf<ArgumentException>());
        }

        [Test]
        public void CheckOut_NoParkingPlaceWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.CheckOut(_nonExisting), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CheckOut_ExistingParkingPlace_ReturnsRedirectToActionResult()
        {
            _parkingPlace.Reservation = _reservation;
            _reservation.StateOfReservationId = StateOfReservation.Inhouse;

            _mockParkingPlaceRepository.Setup(p => p.GetParkingPlaceReservationCar(_existing)).Returns(_parkingPlace);

            var result = (RedirectToRouteResult)_controller.CheckOut(_existing);

            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result.RouteValues.ContainsValue("Parking"));
        }

        [Test]
        public void TemporaryLeave_NoParkingPlaceWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.TemporaryLeave(_nonExisting), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void TemporaryLeave_InhouseReservation_ReturnRedirectToActionResult()
        {
            _parkingPlace.Reservation = _reservation;

            _mockParkingPlaceRepository.Setup(p => p.GetParkingPlaceReservationCar(_existing)).Returns(_parkingPlace);

            var result = (RedirectToRouteResult)_controller.TemporaryLeave(_existing);

            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result.RouteValues.ContainsValue("Parking"));
        }

        [Test]
        public void Reserve_NoReservationWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.Reserve("_existing", _nonExisting), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Reserve_ReservedReservationAndReservedParkingPlace_ReturnRedirectToActionResult()
        {
            _parkingPlace.Reservation = _reservation;
            _mockReservationRepository.Setup(r => r.GetReservation(_existing)).Returns(_reservation);
            _mockParkingPlaceRepository.Setup(r => r.GetParkingPlace(_reservation)).Returns(_parkingPlace);
            _mockParkingPlaceRepository.Setup(r => r.GetParkingPlace("_existing")).Returns(_parkingPlace);
            _mockStatesOfPlacesRepository.Setup(r => r.GetFreeStateOfPlace()).Returns(new StateOfPlace() { Id = _existing});

            var result = (RedirectToRouteResult)_controller.Reserve("_existing",_existing);

            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result.RouteValues.ContainsValue("Parking"));
        }

        [Test]
        public void Reserve_InhouseReservationAndOccupiedParkingPlace_ReturnRedirectToActionResult()
        {
            _parkingPlace.Reservation = _reservation;
            _reservation.StateOfReservationId = StateOfReservation.Inhouse;
            _mockReservationRepository.Setup(r => r.GetReservation(_existing)).Returns(_reservation);
            _mockParkingPlaceRepository.Setup(r => r.GetParkingPlace(_reservation)).Returns(_parkingPlace);
            _mockParkingPlaceRepository.Setup(r => r.GetParkingPlace("_existing")).Returns(_parkingPlace);
            _mockStatesOfPlacesRepository.Setup(r => r.GetFreeStateOfPlace()).Returns(new StateOfPlace() { Id = _existing });

            var result = (RedirectToRouteResult)_controller.Reserve("_existing", _existing);

            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result.RouteValues.ContainsValue("Parking"));
        }
    }
}
