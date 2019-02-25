﻿using System;
using System.ComponentModel.DataAnnotations;

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
            if (this.StateOfReservationId == StateOfReservation.Inhouse)
            {
                this.Departure = DateTime.Now;
                this.ParkingPlaceId = 0;
                this.StateOfReservationId = StateOfReservation.Departed;
                this.Car.AddStay();
            }
            else {
                throw new ArgumentOutOfRangeException("Predana rezervace, ktera neni inhouse.");
            }
        }

        public void TemporaryLeave()
        {
            this.StateOfReservationId = StateOfReservation.TemporaryLeave;
        }

        public void CheckIn()
        {
            if (this.StateOfReservationId == StateOfReservation.Reserved ||
                this.StateOfReservationId == StateOfReservation.TemporaryLeave)
            {
                if (this.StateOfReservationId == StateOfReservation.Reserved)
                    this.Arrival = DateTime.Now;

                if (this.Car.IsEmployee)
                    this.IsRegistered = true;

                this.StateOfReservationId = StateOfReservation.Inhouse;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Predana rezervace se spatnym stavem.");
            }
            
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
            this.Car.ResetPricePerNightToNull();
        }

        public void UpdateInhouseReservationCheckout()
        {
            this.Departure = DateTime.Today.Date + new TimeSpan(12,0,0);
            this.IsRegistered = false;
        }
    }

    
}