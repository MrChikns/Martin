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

        public static string AssignStateOfPlaceName(ParkingPlace parkingPlace, int staffPPlaceId)
        {
            switch (parkingPlace.StateOfPlace.Name)
            {
                case "Obsazeno":
                    if (!parkingPlace.Reservation.IsRegistered)
                    {
                        parkingPlace.StateOfPlace.Name = "Neregistrován!";
                        break;
                    }
                    if (parkingPlace.Reservation.Departure.Date == DateTime.Today.Date)
                        parkingPlace.StateOfPlace.Name = "Odjezd";
                    break;
                case "Volno":
                    if (staffPPlaceId >= 19)
                        parkingPlace.StateOfPlace.Name = "Volno Staff";
                    break;
            }
            return parkingPlace.StateOfPlace.Name;
        }

        internal void Release(StateOfPlace freePlace)
        {
            this.Reservation.ParkingPlaceId = 0;
            this.Reservation = null;
            this.StateOfPlaceId = StateOfPlace.Free;
            this.StateOfPlace = freePlace;
        }

        internal void Reserve(StateOfPlace reservedPlace, Reservation reservation)
        {
            reservation.ParkingPlaceId = this.Id;
            this.StateOfPlaceId = StateOfPlace.Reserved;
            this.StateOfPlace = reservedPlace;
            this.Reservation = reservation;
        }

        internal void Occupy(StateOfPlace occupiedPlaceState, Reservation reservation)
        {
            reservation.ParkingPlaceId = this.Id;
            this.StateOfPlaceId = StateOfPlace.Occupied;
            this.StateOfPlace = occupiedPlaceState;
            this.Reservation = reservation;
        }

        internal void Free(StateOfPlace stateOfPlace, Reservation reservation)
        {
            reservation.ParkingPlaceId = 0;
            this.StateOfPlaceId = StateOfPlace.Free;
            this.StateOfPlace = StateOfPlace;
            this.Reservation = null;
        }
    }
}