using HotelGarage.Models;
using HotelGarage.Repositories;
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
        public string RoomNumberBootbox { get; internal set; }
        public string CarModelBootbox { get; internal set; }
        public string PricePerNightBootbox { get; internal set; }
        public string LicensePlateBootbox { get; internal set; }
        public string IsEmployeeBootbox { get; internal set; }
        public string IsRegisteredBootbox { get; internal set; }
        public string ParkPlaceShortLicensePlate { get; internal set; }

        //constructor for initial empty parkingplace
        public ParkingPlaceDto(ParkingPlace parkingPlace)
        {
            Id = parkingPlace.Id;
            ReservationId = 0;
            LicensePlate = " ";
            Departure = " ";
            PPlaceName = parkingPlace.Name;
            StateOfPlace = parkingPlace.AssignStateOfPlaceName();

            DepartureBootbox = " ";
            IsRegisteredBootbox = " ";
            ArrivalBootbox = " ";
            GuestNameBootbox = " ";
            RoomNumberBootbox = " ";
            CarModelBootbox = " ";
            PricePerNightBootbox = " ";
            LicensePlateBootbox = " ";
            IsEmployeeBootbox = "Host";
        }

        internal void AssignCar(Car car)
        {
            if (car != null)
            {
                this.GuestNameBootbox = (car.GuestsName == null) ? "Nevyplněno" : car.GuestsName;
                this.RoomNumberBootbox = (car.GuestRoomNumber == null) ? "Nevyplněno" : car.GuestRoomNumber.ToString();
                this.CarModelBootbox = (car.CarModel == null) ? "Nevyplněno" : car.CarModel;
                this.PricePerNightBootbox = (car.PricePerNight == null) ? "Nevyplněno" : car.PricePerNight.ToString();
                this.IsEmployeeBootbox = (car.IsEmployee == true) ? "Zaměstnanec" : "Host";
            }
        }

        internal void AssignReservation(ParkingPlace parkingPlace)
        {
            this.LicensePlate = parkingPlace.Reservation.LicensePlate;
            this.LicensePlateBootbox = parkingPlace.Reservation.LicensePlate;
            this.Departure = parkingPlace.Reservation.Departure.ToShortDateString();
            this.DepartureBootbox = parkingPlace.Reservation.Departure.ToShortDateString();
            this.ArrivalBootbox = parkingPlace.Reservation.Arrival.ToShortDateString();
            this.ReservationId = parkingPlace.Reservation.Id;
            this.IsRegisteredBootbox = (parkingPlace.Reservation.IsRegistered)?"Ano":"Ne!";
        }

        public static List<ParkingPlaceDto> GetParkingPlaceDtos(ParkingPlaceRepository parkingPlaceRepository, 
            StateOfPlaceRepository stateOfPlaceRepository, CarRepository carRepository,ApplicationDbContext context)
        {
            var dtoList = new List<ParkingPlaceDto>();
            var parkingPlaces = parkingPlaceRepository.GetParkingPlacesStateOfPlaceReservationCar();
            foreach (var parkingPlace in parkingPlaces)
            {
                //predvyplneni pro prázdné parkovací místo 
                var ppDto = new ParkingPlaceDto(parkingPlace);

                // pokud je potreba vyplnit rezervaci do parkovaciho mista
                if (parkingPlace.Reservation != null)
                {
                    // pokud auto prebydli noc, prestoze melo odjet, posune se jeho datum odjezdu
                    if (parkingPlace.Reservation.Departure < DateTime.Today.Date)
                    {
                        parkingPlace.Reservation.UpdateCheckout();
                        ppDto.StateOfPlace = parkingPlace.AssignStateOfPlaceName();
                        context.SaveChanges();
                    }

                    //vyrazeni rezervaci z minuleho dne anebo prirazeni rezervace do parkovaciho mista
                    if (parkingPlace.Reservation.Arrival.Date != DateTime.Today.Date
                        && parkingPlace.Reservation.StateOfReservationId == StateOfReservation.Reserved)
                    {
                        var res = parkingPlace.Reservation;
                        parkingPlace.Release(stateOfPlaceRepository.GetFreeStateOfPlace());
                        context.SaveChanges();
                    }
                    else
                    {
                        ppDto.AssignCar(carRepository.GetCar(parkingPlace));
                        ppDto.AssignReservation(parkingPlace);
                    }
                }
                dtoList.Add(ppDto);
            }
            return dtoList;
        }


    }
}