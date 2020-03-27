using HotelGarage.Helpers;
using System;

namespace HotelGarage.Core.Models
{
    public class ParkingPlace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public StateOfPlaceEnum State { get; set; }
        public Reservation Reservation { get; set; }

        public void AssignReservation(Reservation reservation)
        {
            Reservation = reservation;
        }

        public void AssignStateOfPlaceId(StateOfPlaceEnum stateOfPlace)
        {
            State = stateOfPlace;
        }

        public void AssignStateOfPlace(StateOfPlaceEnum stateOfPlace)
        {
            State = stateOfPlace;
        }

        public void Release()
        {
            Reservation.SetParkingPlaceId(0);
            State = StateOfPlaceEnum.Free;
            Reservation = null;
        }

        public void Reserve(Reservation reservation)
        {
            reservation.SetParkingPlaceId(Id);
            State = StateOfPlaceEnum.Reserved;
            Reservation = reservation;
        }

        public void MoveInhouseReservation(Reservation reservation)
        {
            if (reservation.State == StateOfReservationEnum.Inhouse)
            {
                reservation.SetParkingPlaceId(Id);
                State = StateOfPlaceEnum.Occupied;
                Reservation = reservation;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid reservation. It has to be inhouse to move it!");
            }
        }

        public void Occupy(Reservation reservation)
        {
            if (State == StateOfPlaceEnum.Reserved && reservation.ParkingPlaceId == Id)
            {
                State = StateOfPlaceEnum.Occupied;
                Reservation = reservation;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid parking place and reservation. Parking place has to be reserved with current reservation.");
            }
        }

        public void AssingnFreeParkingPlace(Reservation reservation)
        {   
            reservation.SetParkingPlaceId(0);
            State = StateOfPlaceEnum.Free;
            Reservation = null;
        }
    }
}