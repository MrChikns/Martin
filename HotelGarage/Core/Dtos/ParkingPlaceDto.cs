﻿using HotelGarage.Core.Models;
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
        public string ParkingPlaceState { get; set; }
        public BootBoxDataDto BootBoxData { get; set; }

        public List<ParkingPlaceDto> GetParkingPlaceDtos(IUnitOfWork unitOfWork)
        {
            var parkingPlaceDtos = new List<ParkingPlaceDto>();
            var parkingPlaces = unitOfWork.ParkingPlaces.GetAllParkingPlaces();

            foreach (var parkingPlace in parkingPlaces)
            {
                var parkingPlaceDto = new ParkingPlaceDto()
                {
                    Id = parkingPlace.Id,
                    ParkingPlaceName = parkingPlace.Name,
                    ParkingPlaceState = GetParkingPlaceLabel(parkingPlace),
                    ReservationId = null,
                    Departure = Labels.Empty,
                    LicensePlate = Labels.Empty,
                    BootBoxData = new BootBoxDataDto()
                };

                if (parkingPlace.Reservation != null)
                {
                    // If car stays to next day after departure date, update the reservation departure.
                    if (parkingPlace.Reservation.Departure < DateTime.Today.Date)
                    {
                        parkingPlace.Reservation.UpdateInhouseReservationCheckout();
                        parkingPlaceDto.StateOfPlace = parkingPlace.State;
                        parkingPlaceDto.ParkingPlaceState = GetParkingPlaceLabel(parkingPlace);

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
                        parkingPlaceDto.AssignReservation(parkingPlace.Reservation);
                    }
                }
                parkingPlaceDtos.Add(parkingPlaceDto);
            }
            return parkingPlaceDtos;
        }

        private string GetParkingPlaceLabel(ParkingPlace parkingPlace)
        {
            switch (parkingPlace.State)
            {
                case Models.ParkingPlaceState.Free:
                    var numberOfStandardParkingPlaces = 19;
                    if (parkingPlace.Id > numberOfStandardParkingPlaces)
                    {
                        return Labels.StaffFreeState;
                    }
                    else
                    {
                        return Labels.FreeState;
                    }
                case Models.ParkingPlaceState.Occupied:
                    if (!parkingPlace.Reservation.IsRegistered)
                    {
                        return Labels.NotRegisteredState;
                    }
                    if (parkingPlace.Reservation.Departure.Date <= DateTime.Today.Date)
                    {
                        return Labels.DepartureState;
                    }
                    return Labels.OccupiedState;
                case Models.ParkingPlaceState.Reserved:
                    return Labels.ReservedState;
                case Models.ParkingPlaceState.Employee:
                    return Labels.EmployeeState;
                default:
                    throw new ArgumentException("Invalid reservation state.");
            }
        }

        internal void AssignCar(Car car)
        {
            if (car != null)
            {
                var notFilledOut = Labels.NotFilledOut;
                BootBoxData.GuestName = car.GuestsName ?? notFilledOut;
                BootBoxData.RoomNumber = (car.GuestRoomNumber == null) ? notFilledOut : car.GuestRoomNumber.ToString();
                BootBoxData.CarModel = car.CarModel ?? notFilledOut;
                BootBoxData.PricePerNight = (car.PricePerNight == null) ? notFilledOut : car.PricePerNight.ToString();
                BootBoxData.IsEmployee = (car.IsEmployee == true) ? "Zaměstnanec" : "Host";
                BootBoxData.Note = car.Note ?? notFilledOut;
            }
        }

        private void AssignReservation(Reservation reservation)
        {
            ReservationId = reservation.Id;
            LicensePlate = reservation.LicensePlate;
            Departure = reservation.Departure.ToShortDateString();
            BootBoxData.Arrival = reservation.Arrival.ToShortDateString();
            BootBoxData.Departure = reservation.Departure.ToShortDateString();
            BootBoxData.LicensePlate = reservation.LicensePlate;
            BootBoxData.IsRegistered = IsRegisteredLabel(reservation.IsRegistered);
        }

        private string IsRegisteredLabel(bool isRegistered)
        {
            return isRegistered ? "Ano" : "Ne!";
        }

        
    }
}