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
            this.Reservation.ParkingPlaceId = 0;
            this.Reservation = null;
            this.StateOfPlaceId = StateOfPlace.Free;
            this.StateOfPlace = freePlace;
        }

        public void Reserve(StateOfPlace reservedPlace, Reservation reservation)
        {
            reservation.ParkingPlaceId = this.Id;
            this.StateOfPlaceId = StateOfPlace.Reserved;
            this.StateOfPlace = reservedPlace;
            this.Reservation = reservation;
        }

        public void MoveInhouseReservation(StateOfPlace inhousePlace, Reservation reservation)
        {
            reservation.ParkingPlaceId = this.Id;
            this.StateOfPlaceId = StateOfPlace.Occupied;
            this.StateOfPlace = inhousePlace;
            this.Reservation = reservation;
        }

        public void Occupy(StateOfPlace occupiedPlaceState, Reservation reservation)
        {
            reservation.ParkingPlaceId = this.Id;
            this.StateOfPlaceId = StateOfPlace.Occupied;
            this.StateOfPlace = occupiedPlaceState;
            this.Reservation = reservation;
        }

        public void Free(StateOfPlace stateOfPlace, Reservation reservation)
        {   
            reservation.ParkingPlaceId = 0;
            this.StateOfPlaceId = StateOfPlace.Free;
            this.StateOfPlace = stateOfPlace;
            this.Reservation = null;
        }
    }
}