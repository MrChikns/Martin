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
            var parkingPlaceName = StateOfPlace.Name;

            switch (StateOfPlace.Name)
            {
                case Constants.ReservedStateOfPlaceLabel:
                case Constants.EmployeeStateOfPlaceLabel:
                    break;
                case Constants.OccupiedStateOfPlaceLabel:
                    if (!Reservation.IsRegistered)
                    {
                        parkingPlaceName = Constants.NotRegisteredStateOfPlaceLabel;
                        break;
                    }
                    if (Reservation.Departure.Date <= DateTime.Today.Date)
                        parkingPlaceName = Constants.DepartureStateOfPlaceLabel;
                    break;
                case Constants.FreeStateOfPlaceLabel:
                    if (Id > Constants.NumberOfStandardParkingPlaces)
                        parkingPlaceName = Constants.FreeStaffStateOfPlaceLabel;
                    break;
                default:
                    throw new ArgumentException("Parking place label is not in the database.");
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
            if (StateOfPlaceId != StateOfPlace.Free)
            {
                Reservation.SetParkingPlaceId(0);
                Reservation = null;
                StateOfPlaceId = StateOfPlace.Free;
                StateOfPlace = freePlace;
            }
        }

        public void Reserve(StateOfPlace reservedPlace, Reservation reservation)
        {
            reservation.SetParkingPlaceId(Id);
            StateOfPlaceId = StateOfPlace.Reserved;
            StateOfPlace = reservedPlace;
            Reservation = reservation;
        }

        public void MoveInhouseReservation(StateOfPlace inhousePlace, Reservation reservation)
        {
            if (reservation.StateOfReservationId == StateOfReservation.Inhouse)
            {
                reservation.SetParkingPlaceId(Id);
                StateOfPlaceId = StateOfPlace.Occupied;
                StateOfPlace = inhousePlace;
                Reservation = reservation;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid reservation. It has to be inhouse to move it!");
            }
        }

        public void Occupy(StateOfPlace occupiedPlaceState, Reservation reservation)
        {
            if (StateOfPlaceId == StateOfPlace.Reserved && reservation.ParkingPlaceId == Id)
            {
                StateOfPlaceId = StateOfPlace.Occupied;
                StateOfPlace = occupiedPlaceState;
                Reservation = reservation;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid parking place and reservation. Parking place has to be reserved with current reservation.");
            }
        }

        public void AssingnFreeParkingPlace(StateOfPlace stateOfPlace, Reservation reservation)
        {   
            reservation.SetParkingPlaceId(0);
            StateOfPlaceId = StateOfPlace.Free;
            StateOfPlace = stateOfPlace;
            Reservation = null;
        }
    }
}