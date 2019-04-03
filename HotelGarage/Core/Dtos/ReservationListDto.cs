using HotelGarage.Core.Models;
using HotelGarage.Core.Repositories;
using HotelGarage.Persistence;
using HotelGarage.Persistence.Repositories;
using System.Collections.Generic;

namespace HotelGarage.Core.Dtos
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

        public ReservationListDto(Reservation reservation, IReservationRepository reservationRepository,
            IParkingPlaceRepository parkingPlaceRepository)
        {
            var nevyplneno = "Nevyplněno";

            GuestsName = reservation.Car.GuestsName ?? nevyplneno;
            Arrival = reservation.Arrival.ToString("yyyy.MM.dd hh:mm");
            Departure = reservation.Departure.ToString("yyyy.MM.dd hh:mm");
            GuestRoomNumber = (reservation.Car.GuestRoomNumber == null) ? 
                nevyplneno : reservation.Car.GuestRoomNumber.ToString();
            TotalPrice = reservation.Car.ReturnCalculatedTotalPriceString(
                reservation.Car.CalculateNumberOfDays(reservation.Arrival, reservation.Departure), 
                reservation.Car.PricePerNight);
            ReservationState = reservationRepository.GetStateOfReservationName(reservation.StateOfReservationId);
            LicensePlate = reservation.LicensePlate;
            CarModel = reservation.Car.CarModel ?? nevyplneno;
            ParkingPlaceName = (reservation.ParkingPlaceId == 0) ? nevyplneno : parkingPlaceRepository.GetParkingPlaceName(reservation.ParkingPlaceId);
            IsEmployee = (reservation.Car.IsEmployee) ? "Zaměstnanec" : "Host";
            NumberOfStays = reservation.Car.NumberOfStays.ToString();
        }

        public static IList<ReservationListDto> GetAllReservationDtos(IUnitOfWork unitOfWork)
        {
            var allResListDto = new List<ReservationListDto>();

            foreach (var res in unitOfWork.Reservations.GetAllReservationsCar())
            {
                allResListDto.Add(new ReservationListDto(res,unitOfWork.Reservations,unitOfWork.ParkingPlaces));
            }
            return allResListDto;
        }

        
    }
}