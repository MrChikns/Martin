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

        public static string AssignStateOfPlaceName(string stateOfPlaceName, 
            ParkingPlace parkingPlace, int staffPPlaceId)
        {
            switch (stateOfPlaceName)
            {
                case "Obsazeno":
                    if (!parkingPlace.Reservation.IsRegistered)
                    {
                        stateOfPlaceName = "Neregistrován!";
                        break;
                    }
                    if (parkingPlace.Reservation.Departure.Date == DateTime.Today.Date)
                        stateOfPlaceName = "Odjezd";
                    break;
                case "Volno":
                    if (staffPPlaceId >= 19)
                        stateOfPlaceName = "Volno Staff";
                    break;
            }
            return stateOfPlaceName;
        }

        internal void Release(StateOfPlace freePlace)
        {
            this.Reservation = null;
            this.StateOfPlaceId = StateOfPlace.Free;
            this.StateOfPlace = freePlace;
        }

        internal void Reserve(StateOfPlace reservedPlace, Reservation reservation)
        {
            this.Reservation = reservation;
            this.StateOfPlaceId = StateOfPlace.Reserved;
            this.StateOfPlace = reservedPlace;
        }

        internal void Occupy(StateOfPlace occupiedPlaceState, Reservation reservation)
        {
            this.StateOfPlaceId = StateOfPlace.Occupied;
            this.StateOfPlace = occupiedPlaceState;
            this.Reservation = reservation;
        }
    }
}