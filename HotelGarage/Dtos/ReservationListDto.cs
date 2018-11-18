using HotelGarage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.Dtos
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
        public string IsEmployee{ get; set; }

        public ReservationListDto(string guestsName, string arrival, string departure, string guestRoomNumber,
            string totalPrice, string reservationState, string licensePlate, string carModel, 
            string parkingPlaceName, string isEmployee )
        {
            GuestsName = guestsName;
            Arrival = arrival;
            Departure = departure;
            GuestRoomNumber = guestRoomNumber;
            TotalPrice = totalPrice;
            ReservationState = reservationState;
            LicensePlate = licensePlate;
            CarModel = carModel;
            ParkingPlaceName = parkingPlaceName;
            IsEmployee = isEmployee;
        }

        public static IList<ReservationListDto> GetAllReservationDtos(ReservationRepository reservationRepository,
            ParkingPlaceRepository parkingPlaceRepository)
        {
            var allResListDto = new List<ReservationListDto>();

            foreach (var res in reservationRepository.GetAllReservationsCar())
            {
                var nevyplneno = "Nevyplněno";
                var name = (res.Car.GuestsName == null) ? nevyplneno: res.Car.GuestsName;
                var totalPrice = (res.Car.PricePerNight == null)?nevyplneno:((res.Departure - res.Arrival).TotalDays * res.Car.PricePerNight).ToString();
                var guestRoomNumber = (res.Car.GuestRoomNumber == null) ? nevyplneno :res.Car.GuestRoomNumber.ToString();
                var carModel = (res.Car.CarModel == null) ? nevyplneno : res.Car.CarModel;
                var reservationState = reservationRepository.GetStateOfReservationName(res.StateOfReservationId);
                var parkingPlaceName = (res.ParkingPlaceId == 0)? nevyplneno :parkingPlaceRepository.GetParkingPlaceName(res.ParkingPlaceId);

                allResListDto.Add(new ReservationListDto(name, res.Arrival.ToString("yyyy.MM.dd hh:mm"),
                    res.Departure.ToString("yyyy.MM.dd hh:mm"), guestRoomNumber, totalPrice, reservationState, 
                    res.LicensePlate, carModel, parkingPlaceName, (res.Car.IsEmployee) ? "Zaměstnanec" : "Host"));
            }
            return allResListDto;
        }
    }
}