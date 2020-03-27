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

        [Required(ErrorMessage = "Zadejte datum ve správném formátu.")]
        [Display(Name = "Příjezd")]
        public DateTime Arrival { get; set; }

        [Required(ErrorMessage = "Zadejte datum ve správném formátu.")]
        [Display(Name = "Odjezd")]
        public DateTime Departure { get; set; }

        [Required]
        public bool IsRegistered { get; set; }

        public int ParkingPlaceId { get; set; }

        public byte StateOfReservationId { get; set; }

        public void SetDepartureDay(DateTime date)
        {
            Departure = date;
        }

        public void SetParkingPlaceId(int id)
        {
            ParkingPlaceId = id;
        }
        
        public void CheckOut()
        {
            if (StateOfReservationId == StateOfReservation.Inhouse)
            {
                Departure = DateTime.Now;
                ParkingPlaceId = 0;
                StateOfReservationId = StateOfReservation.Departed;
                Car.AddStay();
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid reservation state.");
            }
        }

        public void TemporaryLeave()
        {
            StateOfReservationId = StateOfReservation.TemporaryLeave;
        }

        public void CheckIn()
        {
            if (StateOfReservationId == StateOfReservation.Reserved || StateOfReservationId == StateOfReservation.TemporaryLeave)
            {
                if (StateOfReservationId == StateOfReservation.Reserved)
                {
                    Arrival = DateTime.Now;
                }

                if (Car.IsEmployee)
                {
                    IsRegistered = true;
                }

                StateOfReservationId = StateOfReservation.Inhouse;
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
            StateOfReservationId = updated.StateOfReservationId;
        }

        public void Cancel(ParkingPlace parkingPlace, StateOfPlace freePlace)
        {
            if (parkingPlace != null)
            {
                parkingPlace.Release(freePlace);
            }
            
            StateOfReservationId = StateOfReservation.Cancelled;
            Car.ResetPricePerNightToNull();
        }

        public void UpdateInhouseReservationCheckout()
        {
            Departure = DateTime.Today.Date + new TimeSpan(12,0,0);
            IsRegistered = false;
        }
    }

    
}