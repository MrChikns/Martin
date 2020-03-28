using HotelGarage.Controllers;
using HotelGarage.Core;
using HotelGarage.Core.Models;
using HotelGarage.Core.Repositories;
using HotelGarage.UnitTests.Extensions;
using Moq;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace HotelGarage.UnitTests.Controllers
{
    [TestFixture]
    public class ParkingControllerTests
    {
        private ParkingController _controller;
        Mock<IUnitOfWork> _mockUnitOfWork;
        Mock<IReservationRepository> _mockReservationRepository;
        private Mock<IParkingPlaceRepository> _mockParkingPlaceRepository;
        private Reservation _reservation;
        private ParkingPlace _parkingPlace;
        private int _existingId = 1;
        private int _nonExistingId = 2;
        
        [SetUp]
        public void SetUp()
        {
            _reservation = new Reservation()
            {
                Id = _existingId,
                Arrival = DateTime.Today,
                State = ReservationState.Reserved,
                Car = new Car(),
                ParkingPlaceId = _existingId
            };
            _parkingPlace = new ParkingPlace()
            {
                Id = _existingId,
                State = ParkingPlaceState.Reserved,
            };

            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockReservationRepository.Setup(r => r.GetReservation(_existingId)).Returns(_reservation);
            
            _mockParkingPlaceRepository = new Mock<IParkingPlaceRepository>();
            _mockParkingPlaceRepository.Setup(p => p.GetParkingPlace(_existingId)).Returns(_parkingPlace);

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.SetupGet(u => u.Reservations).Returns(_mockReservationRepository.Object);
            _mockUnitOfWork.SetupGet(u => u.ParkingPlaces).Returns(_mockParkingPlaceRepository.Object);

            _controller = new ParkingController(_mockUnitOfWork.Object);
            _controller.MockCurrentUser("1", "user1@domain.com");
        }

        [Test]
        public void CheckIn_ExistingReservationAndParkingPlace_ReturnsRedirectToActionResult()
        {
            var result = (RedirectToRouteResult)_controller.CheckIn(_existingId,_existingId);

            Assert.AreEqual("Parking", result.RouteValues["action"]);
        }

        [Test]
        public void CheckIn_NoReservationWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.CheckIn(_existingId, _nonExistingId),Throws.Exception.TypeOf<ArgumentOutOfRangeException>()); 
        }

        [Test]
        public void CheckIn_NoParkingPlaceWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.CheckIn(_nonExistingId, _existingId), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CheckIn_StateOfPlaceNotReserved_ThrowArgumentException()
        {
            _parkingPlace.State = ParkingPlaceState.Occupied;

            Assert.That(() => _controller.CheckIn(_existingId, _existingId), Throws.Exception.TypeOf<ArgumentException>());
        }

        [Test]
        public void CheckIn_ArrivalNotTodayAndNoTemporaryLeave_ThrowArgumentOutOfRangeException()
        {
            _reservation.Arrival = DateTime.Today.AddDays(1);

            Assert.That(() => _controller.CheckIn(_existingId, _existingId), Throws.Exception.TypeOf<ArgumentException>());
        }

        [Test]
        public void CheckOut_NoParkingPlaceWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.CheckOut(_nonExistingId), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CheckOut_ExistingParkingPlace_ReturnsRedirectToActionResult()
        {
            _parkingPlace.Reservation = _reservation;
            _reservation.State = ReservationState.Inhouse;
            _mockParkingPlaceRepository.Setup(p => p.GetParkingPlace(_existingId)).Returns(_parkingPlace);

            var result = (RedirectToRouteResult)_controller.CheckOut(_existingId);
            Assert.AreEqual("Parking", result.RouteValues["action"]);
        }

        [Test]
        public void TemporaryLeave_NoParkingPlaceWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.TemporaryLeave(_nonExistingId), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void TemporaryLeave_InhouseReservation_ReturnRedirectToActionResult()
        {
            _parkingPlace.Reservation = _reservation;
            _mockParkingPlaceRepository.Setup(p => p.GetParkingPlace(_existingId)).Returns(_parkingPlace);

            var result = (RedirectToRouteResult)_controller.TemporaryLeave(_existingId);
            Assert.AreEqual("Parking", result.RouteValues["action"]);
        }

        [Test]
        public void Reserve_NoReservationWithGivenId_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => _controller.Reserve("_existing", _nonExistingId), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Reserve_ReservedReservationAndReservedParkingPlace_ReturnRedirectToActionResult()
        {
            _parkingPlace.Reservation = _reservation;
            _mockReservationRepository.Setup(r => r.GetReservation(_existingId)).Returns(_reservation);
            _mockParkingPlaceRepository.Setup(r => r.GetParkingPlace(_reservation)).Returns(_parkingPlace);
            _mockParkingPlaceRepository.Setup(r => r.GetParkingPlace("_existing")).Returns(_parkingPlace);

            var result = (RedirectToRouteResult)_controller.Reserve("_existing",_existingId);
            Assert.AreEqual("Parking", result.RouteValues["action"]);
        }

        [Test]
        public void Reserve_InhouseReservationAndOccupiedParkingPlace_ReturnRedirectToActionResult()
        {
            _parkingPlace.Reservation = _reservation;
            _reservation.State = ReservationState.Inhouse;
            _mockReservationRepository.Setup(r => r.GetReservation(_existingId)).Returns(_reservation);
            _mockParkingPlaceRepository.Setup(r => r.GetParkingPlace(_reservation)).Returns(_parkingPlace);
            _mockParkingPlaceRepository.Setup(r => r.GetParkingPlace("_existing")).Returns(_parkingPlace);

            var result = (RedirectToRouteResult)_controller.Reserve("_existing", _existingId);
            Assert.AreEqual("Parking", result.RouteValues["action"]);
        }
    }
}
