using System;
using System.ComponentModel.DataAnnotations;

namespace HotelGarage.Core.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "SPZ")]
        [StringLength(20)]
        public string LicensePlate { get; set; }

        public Car Car { get; set; }

        [Required(ErrorMessage = "Invalid date format.")]
        [Display(Name = "Příjezd")]
        public DateTime Arrival { get; set; }

        [Required(ErrorMessage = "Invalid date format.")]
        [Display(Name = "Odjezd")]
        public DateTime Departure { get; set; }

        [Required]
        public bool IsRegistered { get; set; }

        public int ParkingPlaceId { get; set; }

        public ReservationState State { get; set; }

        public void SetParkingPlaceId(int id)
        {
            ParkingPlaceId = id;
        }
        
        public void CheckOut()
        {
            if (State == ReservationState.Inhouse)
            {
                Departure = DateTime.Now;
                ParkingPlaceId = 0;
                State = ReservationState.Departed;
                Car.AddStay();
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid reservation state.");
            }
        }

        public void CheckIn()
        {
            if (State == ReservationState.Reserved || State == ReservationState.TemporaryLeave)
            {
                if (State == ReservationState.Reserved)
                {
                    Arrival = DateTime.Now;
                }

                if (Car.IsEmployee)
                {
                    IsRegistered = true;
                }

                State = ReservationState.Inhouse;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid reservation state.");
            }
            
        }

        public void Update(Reservation updated, Car car)
        {
            Arrival = updated.Arrival;
            Departure = updated.Departure;
            IsRegistered = updated.IsRegistered;
            LicensePlate = updated.LicensePlate;
            ParkingPlaceId = updated.ParkingPlaceId;
            Car = car;
            State = updated.State;
        }

        public void Cancel(ParkingPlace parkingPlace)
        {
            if (parkingPlace != null)
            {
                parkingPlace.Release();
            }

            State = ReservationState.Cancelled;
            Car.ResetPricePerNightToNull();
        }

        public void UpdateInhouseReservationCheckout()
        {
            Departure = DateTime.Today.Date + new TimeSpan(12,0,0);
            IsRegistered = false;
        }
    }
}