using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace HotelGarage.Models
{
    public class ParkingPlace
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Reservation Reservation { get; set; }

        public int StateOfPlaceId { get; set; }

        public StateOfPlace StateOfPlace { get; set; }

        public string AssignStateOfPlaceName()
        {
            string parkingPlaceName = this.StateOfPlace.Name;

            switch (this.StateOfPlace.Name){
                case "Volno":
                case "Obsazeno":
                case "Rezervováno":
                case "Zaměstnanec":
                    break;
                default:
                    throw new ArgumentException("Jmeno parkovaciho mista musi byt jedno z prednastavenych jmen v databazi!");
            }

            switch (this.StateOfPlace.Name)
            {
                case "Obsazeno":
                    if (!this.Reservation.IsRegistered)
                    {
                        parkingPlaceName = "Neregistrován!";
                        break;
                    }
                    if (this.Reservation.Departure.Date <= DateTime.Today.Date)
                        parkingPlaceName = "Odjezd";
                    break;
                case "Volno":
                    if (this.Id > 19)
                        parkingPlaceName = "Volno Staff";
                    break;
            }
            return parkingPlaceName;
        }

        public void Release(StateOfPlace freePlace)
        {
            if (this.StateOfPlaceId != StateOfPlace.Free)
            {
                this.Reservation.ParkingPlaceId = 0;
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
            if (reservation.ParkingPlaceId == 0 && this.StateOfPlaceId == StateOfPlace.Free)
            {
                reservation.ParkingPlaceId = this.Id;
                this.StateOfPlaceId = StateOfPlace.Reserved;
                this.StateOfPlace = reservedPlace;
                this.Reservation = reservation;
            }
            else {
                throw new ArgumentOutOfRangeException("Reservation cannot have assigned parking place " +
                    "and/or parking place has to be free!");
            }
        }

        public void MoveInhouseReservation(StateOfPlace inhousePlace, Reservation reservation)
        {
            if (reservation.StateOfReservationId == StateOfReservation.Inhouse)
            {
                reservation.ParkingPlaceId = this.Id;
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
                //reservation.ParkingPlaceId = this.Id;
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
            reservation.ParkingPlaceId = 0;
            this.StateOfPlaceId = StateOfPlace.Free;
            this.StateOfPlace = stateOfPlace;
            this.Reservation = null;
        }
    }
}