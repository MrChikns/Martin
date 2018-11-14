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
        public int? RoomNumberBootbox { get; internal set; }
        public string CarModelBootbox { get; internal set; }
        public int? PricePerNightBootbox { get; internal set; }
        public string LicensePlateBootbox { get; internal set; }
        public string IsEmployeeBootbox { get; internal set; }
        public string IsRegisteredBootbox { get; internal set; }
        public string ParkPlaceShortLicensePlate { get; internal set; }

        public ParkingPlaceDto(int parkingPlaceId, int? resId, string licensePlate, bool isRegistered,
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
            IsRegisteredBootbox = (isRegistered)?"Ano":"Ne!"; 
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
                var ppDto = new ParkingPlaceDto(parkingPlace.Id, 0, " ", false, " ", parkingPlace.Name,
                    ParkingPlace.AssignStateOfPlaceName(parkingPlace),
                    " ", " ", 0, " ", 0, "Host");

                // pokud je potreba vyplnit rezervaci do parkovaciho mista
                if (parkingPlace.Reservation != null)
                {
                    if (parkingPlace.Reservation.Departure < DateTime.Today.Date)
                    {
                        parkingPlace.Reservation.UpdateCheckout();
                        ppDto.StateOfPlace = ParkingPlace.AssignStateOfPlaceName(parkingPlace);
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