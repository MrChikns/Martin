using System;
using System.ComponentModel.DataAnnotations;

namespace HotelGarage.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "SPZ")]
        public string LicensePlate { get; set; }

        [StringLength(50)]
        [Display(Name = "Typ Auta")]
        public string CarModel { get; set; }

        [StringLength(50)]
        [Display(Name = "Jméno")]
        public string GuestsName { get; set; }

        [Range(101, 528, ErrorMessage = "Zadejte prosím platné číslo pokoje.")]
        [Display(Name = "Číslo Pokoje")]
        public int? GuestRoomNumber { get; set; }

        [Display(Name = "Cena")]
        public int? PricePerNight { get; set; }
        
        public bool IsEmployee { get; set; }

        public Car() { }

        public Car(string licensePlate, string carModel, string guestsName, 
            int? guestRoomNumber, int? pricePerNight, bool isEmployee)
        {
            LicensePlate = licensePlate;
            CarModel = carModel;
            GuestsName = guestsName;
            GuestRoomNumber = guestRoomNumber;
            PricePerNight = pricePerNight;
            IsEmployee = isEmployee;
        }

        internal void Update(Reservation reservation)
        {
            this.LicensePlate = reservation.Car.LicensePlate;
            this.CarModel = reservation.Car.CarModel;
            this.GuestsName = reservation.Car.GuestsName;
            this.GuestRoomNumber = reservation.Car.GuestRoomNumber;
            this.PricePerNight = reservation.Car.PricePerNight;
            this.IsEmployee = reservation.Car.IsEmployee;
        }
    }
}