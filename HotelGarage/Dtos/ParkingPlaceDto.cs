using HotelGarage.Models;
using HotelGarage.Repositories;
using System;
using System.Collections.Generic;

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
        public string NoteBootBox { get; private set; }
        public string IsRegisteredBootbox { get; internal set; }
        public string ParkPlaceShortLicensePlate { get; internal set; }

        //constructor to initialize empty parkingplace
        public ParkingPlaceDto(ParkingPlace parkingPlace)
        {
            Id = parkingPlace.Id;
            ReservationId = 0;
            LicensePlate = " ";
            Departure = " ";
            PPlaceName = parkingPlace.Name;
            StateOfPlace = parkingPlace.GetStateOfPlaceName();

            DepartureBootbox = " ";
            IsRegisteredBootbox = " ";
            ArrivalBootbox = " ";
            GuestNameBootbox = " ";
            RoomNumberBootbox = " ";
            CarModelBootbox = " ";
            PricePerNightBootbox = " ";
            LicensePlateBootbox = " ";
            IsEmployeeBootbox = "Host";
            NoteBootBox = " ";
        }

        internal void AssignCar(Car car)
        {
            if (car != null)
            {
                var notFilledOut = Helpers.Constants.NotFilledOutMessageConstant;

                this.GuestNameBootbox = car.GuestsName ?? notFilledOut;
                this.RoomNumberBootbox = (car.GuestRoomNumber == null) ? notFilledOut : car.GuestRoomNumber.ToString();
                this.CarModelBootbox = car.CarModel ?? notFilledOut;
                this.PricePerNightBootbox = (car.PricePerNight == null) ? notFilledOut : car.PricePerNight.ToString();
                this.IsEmployeeBootbox = (car.IsEmployee == true) ? "Zaměstnanec" : "Host";
                this.NoteBootBox = car.Note ?? notFilledOut;
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
                        parkingPlace.Reservation.UpdateInhouseReservationCheckout();
                        ppDto.StateOfPlace = parkingPlace.GetStateOfPlaceName();
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
                        ppDto.AssignCar(carRepository.GetCar(parkingPlace.Reservation));
                        ppDto.AssignReservation(parkingPlace);
                    }
                }
                dtoList.Add(ppDto);
            }
            return dtoList;
        }
    }
}