using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using HotelGarage.Dtos;
using HotelGarage.ViewModels;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using HotelGarage.Repositories;

namespace HotelGarage.Controllers
{
    public class ParkingController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ParkingPlaceRepository _parkingPlaceRepository;
        private readonly ReservationRepository _reservationRepository;
        private readonly CarRepository _carRepository;
        private readonly StateOfPlaceRepository _stateOfPlaceRepository;

        public ParkingController()
        {
            _context = new ApplicationDbContext();
            _stateOfPlaceRepository = new StateOfPlaceRepository(_context);
            _parkingPlaceRepository = new ParkingPlaceRepository(_context);
            _reservationRepository = new ReservationRepository(_context);
            _carRepository = new CarRepository(_context);
        }

        public ActionResult Parking()
        {
            return View(new ParkingViewModel(
                GetParkingPlaceDtos(), 
                GetArrivingReservations(), 
                GetNoShowReservations(), 
                _parkingPlaceRepository.GetNamesOfFreeParkingPlaces()));
        }

        public ActionResult CheckIn(int pPlaceId, int reservationId)
        {
            var reservation = _reservationRepository.GetReservation(reservationId);

            if (reservation.Arrival.Date != DateTime.Today.Date)
                return RedirectToAction("Parking");

            if (_parkingPlaceRepository.GetParkingPlace(pPlaceId).StateOfPlaceId == StateOfPlace.Reserved)
            {
                reservation.CheckIn();
                _parkingPlaceRepository.GetParkingPlace(pPlaceId)
                    .Occupy(_stateOfPlaceRepository.GetOccupiedStateOfPlace(), reservation);

                _context.SaveChanges();
            }

            return RedirectToAction("Parking");
        }


        public ActionResult CheckOut(int pPlaceId)
        {
            var pPlace = _parkingPlaceRepository.GetParkingPlaceReservation(pPlaceId);

            pPlace.Reservation.CheckOut();
            pPlace.Release(_stateOfPlaceRepository.GetFreeStateOfPlace());

            _context.SaveChanges();

            return RedirectToAction("Parking");
        }

        [HttpPost]
        public ActionResult Reserve(string ParkingPlaceName, int ReservationId)
        {
            var reservation = _reservationRepository.GetReservation(ReservationId);

            // presunuti rezervace z predchoziho prirazeneho mista(pokud uz byla nekde prirazena)
            if (reservation.ParkingPlaceId != 0)
            {
                _parkingPlaceRepository.GetParkingPlace(reservation)
                    .Release(_stateOfPlaceRepository.GetFreeStateOfPlace());
            }
            _parkingPlaceRepository.GetParkingPlace(ParkingPlaceName)
                .Reserve(_stateOfPlaceRepository.GetReservedStateOfPlace(), reservation);

            _context.SaveChanges();

            return RedirectToAction("Parking");
        }

        //method for Parking
        private IList<ReservationDto> GetNoShowReservations()
        {
            var nSResDtos = new List<ReservationDto>();
            foreach (var reservation in _reservationRepository.GetNoShowReservationsCar())
            {
                //asi smazat
                string parkingPlaceName;

                if (reservation.ParkingPlaceId != 0)
                    parkingPlaceName = _parkingPlaceRepository.GetParkingPlace(reservation).Name;
                else
                    parkingPlaceName = "Nepřiřazeno";

                nSResDtos.Add(new ReservationDto(reservation.Id, reservation.Car.LicensePlate,
                    reservation.Car.GuestsName, reservation.ParkingPlaceId, parkingPlaceName,
                    reservation.Arrival.ToShortDateString()));
            }
            return nSResDtos;
        }

        //method for Parking
        private IList<ReservationDto> GetArrivingReservations()
        {
            var arrivingResDtos = new List<ReservationDto>();
            foreach (var reservation in _reservationRepository.GetTodaysReservationsCar())
            {
                string parkingPlaceName;

                if (reservation.ParkingPlaceId != 0)
                    parkingPlaceName = _parkingPlaceRepository.GetParkingPlace(reservation).Name;
                else
                    parkingPlaceName = "Nepřiřazeno";

                arrivingResDtos.Add(new ReservationDto(reservation.Id, reservation.Car.LicensePlate,
                    reservation.Car.GuestsName, reservation.ParkingPlaceId, parkingPlaceName,
                    reservation.Arrival.ToShortDateString()));
            }
            return arrivingResDtos;
        }

        //method for Parking
        private List<ParkingPlaceDto> GetParkingPlaceDtos()
        {
            var dtoList = new List<ParkingPlaceDto>();
            var parkingPlaces = _parkingPlaceRepository.GetParkingPlacesStateOfPlaceReservationCar();
            foreach (var parkingPlace in parkingPlaces)
            {
                //predvyplneni pro prázdné parkovací místo 
                var ppDto = new ParkingPlaceDto(parkingPlace.Id, 0, " ", false ," ", parkingPlace.Name,
                    ParkingPlace.AssignStateOfPlaceName(parkingPlace),
                    " ", " ", 0, " ", 0, "Host");

                // pokud je potreba vyplnit rezervaci do parkovaciho mista
                if (parkingPlace.Reservation != null)
                {
                    //vyrazeni rezervaci z minuleho dne anebo prirazeni rezervace do parkovaciho mista
                    if (parkingPlace.Reservation.Arrival.Date != DateTime.Today.Date
                        && parkingPlace.Reservation.StateOfReservationId == StateOfReservation.Reserved)
                    {
                        var res = parkingPlace.Reservation;
                        parkingPlace.Release(_stateOfPlaceRepository.GetFreeStateOfPlace());
                        _context.SaveChanges();

                    }
                    else
                    {
                        ppDto.AssignCar(_carRepository.GetCar(parkingPlace));
                        ppDto.AssignReservation(parkingPlace);
                    }
                }
                dtoList.Add(ppDto);
            }
            return dtoList;
        }
    }
}