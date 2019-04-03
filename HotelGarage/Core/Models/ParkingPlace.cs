using HotelGarage.Helpers;
using System;

namespace HotelGarage.Core.Models
{
    public class ParkingPlace
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Reservation Reservation { get; set; }

        public int StateOfPlaceId { get; set; }

        public StateOfPlace StateOfPlace { get; set; }

        public string GetStateOfPlaceName()
        {
            string parkingPlaceName = this.StateOfPlace.Name;

            switch (this.StateOfPlace.Name)
            {
                case Constants.ReservedStateOfPlaceConstant:
                case Constants.EmployeeStateOfPlaceConstant:
                    break;
                case Constants.OccupiedStateOfPlaceConstant:
                    if (!this.Reservation.IsRegistered)
                    {
                        parkingPlaceName = Constants.NotRegisteredStateOfPlaceConstant;
                        break;
                    }
                    if (this.Reservation.Departure.Date <= DateTime.Today.Date)
                        parkingPlaceName = Constants.DepartureStateOfPlaceConstant;
                    break;
                case Constants.FreeStateOfPlaceConstant:
                    if (this.Id > Helpers.Constants.NumberOfStandardParkingPlaces)
                        parkingPlaceName = Constants.FreeStaffStateOfPlaceConstant;
                    break;
                default:
                    throw new ArgumentException("Jmeno parkovaciho mista musi byt jedno z prednastavenych jmen v databazi!");
            }
            return parkingPlaceName;
        }

        public void AssignReservation(Reservation reservation)
        {
            Reservation = reservation;
        }

        public void AssignStateOfPlaceId(int id)
        {
            StateOfPlaceId = id;
        }

        public void AssignStateOfPlace(StateOfPlace stateOfPlace)
        {
            StateOfPlace = stateOfPlace;
        }

        public void Release(StateOfPlace freePlace)
        {
            if (this.StateOfPlaceId != StateOfPlace.Free)
            {
                this.Reservation.SetParkingPlaceId(0);
                this.Reservation = null;
                this.StateOfPlaceId = StateOfPlace.Free;
                this.StateOfPlace = freePlace;
            }
            else {
                throw new ArgumentOutOfRangeException("Parking place needs to have assigned reservation!");
            }
        }

        public void Reserve(StateOfPlace reservedPlace, Reservation reservation)
        {
                reservation.SetParkingPlaceId(this.Id);
                this.StateOfPlaceId = StateOfPlace.Reserved;
                this.StateOfPlace = reservedPlace;
                this.Reservation = reservation;
        }

        public void MoveInhouseReservation(StateOfPlace inhousePlace, Reservation reservation)
        {
            if (reservation.StateOfReservationId == StateOfReservation.Inhouse)
            {
                reservation.SetParkingPlaceId(this.Id);
                this.StateOfPlaceId = StateOfPlace.Occupied;
                this.StateOfPlace = inhousePlace;
                this.Reservation = reservation;
            }
            else {
                throw new ArgumentOutOfRangeException("Reservation has to be inhouse to move it " +
                    "to another parking place!");
            }
        }

        public void Occupy(StateOfPlace occupiedPlaceState, Reservation reservation)
        {
            if (this.StateOfPlaceId == StateOfPlace.Reserved && reservation.ParkingPlaceId == this.Id)
            {
                this.StateOfPlaceId = StateOfPlace.Occupied;
                this.StateOfPlace = occupiedPlaceState;
                this.Reservation = reservation;
            }
            else {
                throw new ArgumentOutOfRangeException("State of place has to be set to reserved and/or " +
                    "reservation has to be assigned to the parking place you want to check in!");
            }
        }

        public void AssingnFreeParkingPlace(StateOfPlace stateOfPlace, Reservation reservation)
        {   
            reservation.SetParkingPlaceId(0);
            this.StateOfPlaceId = StateOfPlace.Free;
            this.StateOfPlace = stateOfPlace;
            this.Reservation = null;
        }
    }
}