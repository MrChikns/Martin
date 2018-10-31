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

        internal void CheckOut()
        {
            this.Departure = DateTime.Now;
            this.ParkingPlaceId = 0;
            this.StateOfReservationId = StateOfReservation.Departed;
        }

        internal void CheckIn()
        {
            this.StateOfReservationId = StateOfReservation.Inhouse;
            this.Arrival = DateTime.Now;
        }
    }

    
}