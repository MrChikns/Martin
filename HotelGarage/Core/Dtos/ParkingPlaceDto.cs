using HotelGarage.Core.Models;
using HotelGarage.Helpers;
using System;
using System.Collections.Generic;

namespace HotelGarage.Core.Dtos
{
    public class ParkingPlaceDto
    {
        public int Id { get; set; }
        public int? ReservationId { get; set; }
        public string LicensePlate { get; set; }
        public string Departure { get; set; }
        public string ParkingPlaceName { get; set; }
        public ParkingPlaceState StateOfPlace { get; set; }
        public string StateOfPlaceLabel { get; set; }
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

        public ParkingPlaceDto()
        { }

        private ParkingPlaceDto(ParkingPlace parkingPlace)
        {
            Id = parkingPlace.Id;
            ReservationId = 0;
            LicensePlate = " ";
            Departure = " ";
            ParkingPlaceName = parkingPlace.Name;
            StateOfPlaceLabel = GetParkingPlaceLabel(parkingPlace);
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

        private string GetParkingPlaceLabel(ParkingPlace parkingPlace)
        {
            string label;
            var reservation = parkingPlace.Reservation;

            switch (parkingPlace.State)
            {
                case ParkingPlaceState.Free:
                    var numberOfStandardParkingPlaces = 19;
                    if (Id > numberOfStandardParkingPlaces)
                    {
                        label = Labels.StaffFreeStateLabel;
                    }
                    else
                    {
                        label = Labels.FreeStateLabel;
                    }
                    break;
                case ParkingPlaceState.Occupied:
                    if (!reservation.IsRegistered)
                    {
                        label = Labels.NotRegisteredStateLabel;
                        break;
                    }
                    if (reservation.Departure.Date <= DateTime.Today.Date)
                    {
                        label = Labels.DepartureStateLabel;
                        break;
                    }
                    label = Labels.OccupiedStateLabel;
                    break;
                case ParkingPlaceState.Reserved:
                    label = Labels.ReservedStateLabel;
                    break;
                case ParkingPlaceState.Employee:
                    label = Labels.EmployeeStateLabel;
                    break;
                default:
                    throw new ArgumentException("Invalid reservation state.");
            }

            return label;
        }

        internal void AssignCar(Car car)
        {
            if (car != null)
            {
                var notFilledOut = Helpers.Labels.NotFilledOutLabel;
                GuestNameBootbox = car.GuestsName ?? notFilledOut;
                RoomNumberBootbox = (car.GuestRoomNumber == null) ? notFilledOut : car.GuestRoomNumber.ToString();
                CarModelBootbox = car.CarModel ?? notFilledOut;
                PricePerNightBootbox = (car.PricePerNight == null) ? notFilledOut : car.PricePerNight.ToString();
                IsEmployeeBootbox = (car.IsEmployee == true) ? "Zaměstnanec" : "Host";
                NoteBootBox = car.Note ?? notFilledOut;
            }
        }

        internal void AssignReservation(ParkingPlace parkingPlace)
        {
            LicensePlate = parkingPlace.Reservation.LicensePlate;
            LicensePlateBootbox = parkingPlace.Reservation.LicensePlate;
            Departure = parkingPlace.Reservation.Departure.ToShortDateString();
            DepartureBootbox = parkingPlace.Reservation.Departure.ToShortDateString();
            ArrivalBootbox = parkingPlace.Reservation.Arrival.ToShortDateString();
            ReservationId = parkingPlace.Reservation.Id;
            IsRegisteredBootbox = (parkingPlace.Reservation.IsRegistered)? "Ano" : "Ne!";
        }

        public List<ParkingPlaceDto> GetParkingPlaceDtos(IUnitOfWork unitOfWork)
        {
            var parkingPlaceDtos = new List<ParkingPlaceDto>();
            var parkingPlaces = unitOfWork.ParkingPlaces.GetAllParkingPlaces();

            foreach (var parkingPlace in parkingPlaces)
            {
                var parkingPlaceDto = new ParkingPlaceDto(parkingPlace);
                
                if (parkingPlace.Reservation != null)
                {
                    // pokud auto prebydli noc, prestoze melo odjet, posune se jeho datum odjezdu
                    if (parkingPlace.Reservation.Departure < DateTime.Today.Date)
                    {
                        parkingPlace.Reservation.UpdateInhouseReservationCheckout();
                        parkingPlaceDto.StateOfPlace = parkingPlace.State;
                        parkingPlaceDto.StateOfPlaceLabel = GetParkingPlaceLabel(parkingPlace);
                        unitOfWork.Complete();
                    }

                    //vyrazeni rezervaci z minuleho dne anebo prirazeni rezervace do parkovaciho mista
                    if (parkingPlace.Reservation.Arrival.Date != DateTime.Today.Date && parkingPlace.Reservation.State == ReservationState.Reserved)
                    {
                        parkingPlace.Release();
                        unitOfWork.Complete();
                    }
                    else
                    {
                        parkingPlaceDto.AssignCar(unitOfWork.Cars.GetCar(parkingPlace.Reservation));
                        parkingPlaceDto.AssignReservation(parkingPlace);
                    }
                }
                parkingPlaceDtos.Add(parkingPlaceDto);
            }
            return parkingPlaceDtos;
        }
    }
}