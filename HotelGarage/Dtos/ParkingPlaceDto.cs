using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace HotelGarage.Dtos
{
    public class ParkingPlaceDto
    {
        public int Id { get; set; }
        public int? ReservationId { get; set; }
        public string LicensePlate { get; set; }
        public string Departure { get; set; }
        public string PPlaceName { get; set; }
        public string StateOfPlace { get; set; }

        public string DepartureBootbox { get; set; }
        public string ArrivalBootbox { get; internal set; }
        public string GuestNameBootbox { get; internal set; }
        public int? RoomNumberBootbox { get; internal set; }
        public string CarModelBootbox { get; internal set; }
        public int? PricePerNightBootbox { get; internal set; }
        public string LicensePlateBootbox { get; internal set; }
        public string IsEmployeeBootbox { get; internal set; }
        public string ParkPlaceShortLicensePlate { get; internal set; }

        public ParkingPlaceDto(int parkingPlaceId, int? resId, string licensePlate,
            string departure, string pPlaceName, string stateOfPlaceName, string arrival,
            string pPlaceGuestsName, int? pPRoom, string pPlaceCar, int? pPPrice, string isEmployee)
        {
            Id = parkingPlaceId;
            ReservationId = resId;
            LicensePlate = licensePlate;
            Departure = departure;
            PPlaceName = pPlaceName;
            StateOfPlace = stateOfPlaceName;

            DepartureBootbox = departure.Replace(" ", "_");
            ArrivalBootbox = arrival.Replace(" ", "_");
            GuestNameBootbox = pPlaceGuestsName.Replace(" ", "_");
            RoomNumberBootbox = pPRoom;
            CarModelBootbox = pPlaceCar.Replace(" ", "_");
            PricePerNightBootbox = pPPrice;
            LicensePlateBootbox = licensePlate.Replace(" ", "_");
            IsEmployeeBootbox = isEmployee;
        }

        internal void AssignCar(Car car)
        {
            if (car != null)
            {
                this.GuestNameBootbox = (car.GuestsName == null) ? "Nevyplněno" : car.GuestsName.Replace(" ", "_");
                this.RoomNumberBootbox = (car.GuestRoomNumber == null) ? 0 : car.GuestRoomNumber;
                this.CarModelBootbox = (car.CarModel == null) ? "Nevyplněno" : car.CarModel.Replace(" ", "_");
                this.PricePerNightBootbox = (car.PricePerNight == null) ? 0 : car.PricePerNight;
                this.IsEmployeeBootbox = (car.IsEmployee == true) ? "Zaměstnanec" : "Host";
            }
        }

        internal void AssignReservation(ParkingPlace parkingPlace)
        {
            this.LicensePlate = parkingPlace.Reservation.LicensePlate;
            this.LicensePlateBootbox = parkingPlace.Reservation.LicensePlate.Replace(" ", "_");
            this.Departure = parkingPlace.Reservation.Departure.ToShortDateString();
            this.DepartureBootbox = parkingPlace.Reservation.Departure.ToShortDateString().Replace(" ", "_");
            this.ArrivalBootbox = parkingPlace.Reservation.Arrival.ToShortDateString().Replace(" ", "_"); ;
            this.ReservationId = parkingPlace.Reservation.Id;
        }
    }
}