using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelGarage.Models
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

        public Reservation() { }

        public Reservation(string licensePlate ,DateTime arrival, DateTime departure, 
            bool isRegistered, int parkingPlaceId, Car car)
        {
            LicensePlate = licensePlate;
            Arrival = arrival;
            Departure = departure;
            IsRegistered = isRegistered;
            ParkingPlaceId = parkingPlaceId;
            Car = car;
            StateOfReservationId = StateOfReservation.Reserved;
        }

        public void CheckOut()
        {
            this.Departure = DateTime.Now;
            this.ParkingPlaceId = 0;
            this.StateOfReservationId = StateOfReservation.Departed;
        }

        public void TemporaryLeave()
        {
            this.StateOfReservationId = StateOfReservation.TemporaryLeave;
        }

        public void CheckIn()
        {
            if(this.StateOfReservationId != StateOfReservation.TemporaryLeave)
                this.Arrival = DateTime.Now;

            this.StateOfReservationId = StateOfReservation.Inhouse;
            
        }

        public void Update(Reservation updated, Car car)
        {
            this.Arrival = updated.Arrival;
            this.Departure = updated.Departure;
            this.IsRegistered = updated.IsRegistered;
            this.LicensePlate = updated.LicensePlate;
            this.ParkingPlaceId = updated.ParkingPlaceId;
            this.Car = car;
            this.StateOfReservationId = updated.StateOfReservationId;
        }

        public void Cancel(ParkingPlace parkingPlace, StateOfPlace freePlace)
        {
            if(parkingPlace != null)
                parkingPlace.Release(freePlace);
            
            this.StateOfReservationId = StateOfReservation.Cancelled;
            this.Car.PricePerNight = null;
        }

        public void UpdateCheckout()
        {
            this.Departure = DateTime.Today.Date + new TimeSpan(12,0,0);
            this.IsRegistered = false;
        }
    }

    
}