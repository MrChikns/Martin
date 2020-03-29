using System;

namespace HotelGarage.Core.Models
{
    public class ParkingPlace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ParkingPlaceState State { get; set; }
        public Reservation Reservation { get; set; }

        public void AssignReservation(Reservation reservation)
        {
            Reservation = reservation;
        }

        public void Release()
        {
            Reservation.SetParkingPlaceId(0);
            State = ParkingPlaceState.Free;
            Reservation = null;
        }

        public void Reserve(Reservation reservation)
        {
            reservation.SetParkingPlaceId(Id);
            State = ParkingPlaceState.Reserved;
            Reservation = reservation;
        }

        public void Occupy(Reservation reservation)
        {
            if (State == ParkingPlaceState.Reserved && reservation.ParkingPlaceId == Id)
            {
                State = ParkingPlaceState.Occupied;
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
            State = ParkingPlaceState.Free;
            Reservation = null;
        }
    }
}