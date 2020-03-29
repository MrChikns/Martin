using HotelGarage.Core.Model;
using HotelGarage.Core.Repository;
using HotelGarage.Helpers;
using System.Collections.Generic;

namespace HotelGarage.Core.Dto
{
    public class ReservationListDto
    {
        public string GuestsName { get; set; }
        public string Arrival { get; set; }
        public string Departure { get; set; }
        public string GuestRoomNumber { get; set; }
        public string TotalPrice { get; set; }
        public string ReservationState { get; set; }
        public string LicensePlate { get; set; }
        public string CarModel { get; set; }
        public string ParkingPlaceName { get; set; }
        public string IsEmployee { get; set; }
        public string NumberOfStays { get; set; }
        public int Id { get; set; }

        public ReservationListDto(Reservation reservation, IParkingPlaceRepository parkingPlaceRepository)
        {
            GuestsName = reservation.Car.GuestsName ?? Labels.NotFilledOut;
            Arrival = reservation.Arrival.ToString("yyyy.MM.dd hh:mm");
            Departure = reservation.Departure.ToString("yyyy.MM.dd hh:mm");
            GuestRoomNumber = (reservation.Car.GuestRoomNumber == null) ? Labels.NotFilledOut : reservation.Car.GuestRoomNumber.ToString();
            TotalPrice = reservation.Car.ReturnTotalPriceString(
                reservation.Car.CalculateNumberOfDays(reservation.Arrival, reservation.Departure), 
                reservation.Car.PricePerNight
            );
            ReservationState = reservation.State.ToString();
            LicensePlate = reservation.LicensePlate;
            CarModel = reservation.Car.CarModel ?? Labels.NotFilledOut;
            ParkingPlaceName = (reservation.ParkingPlaceId == 0) ? Labels.NotFilledOut : parkingPlaceRepository.GetParkingPlaceName(reservation.ParkingPlaceId);
            IsEmployee = (reservation.Car.IsEmployee) ? Labels.EmployeeState : Labels.GuestState;
            NumberOfStays = reservation.Car.NumberOfStays.ToString();
            Id = reservation.Id;
        }

        public static IList<ReservationListDto> GetAllReservationDtos(IUnitOfWork unitOfWork)
        {
            var allReservationDtos = new List<ReservationListDto>();
            var allReservations = unitOfWork.Reservations.GetAllReservations();

            foreach (var res in allReservations)
            {
                allReservationDtos.Add(new ReservationListDto(res, unitOfWork.ParkingPlaces));
            }

            return allReservationDtos;
        }
    }
}