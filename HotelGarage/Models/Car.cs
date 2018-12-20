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

        [StringLength(100)]
        [Display(Name = "Poznámka")]
        public string Note { get; set; }

        [Display(Name ="Počet Pobytů")]
        public int NumberOfStays { get; private set; }

        public Car() { }

        public Car(string licensePlate, string carModel, string guestsName, 
            int? guestRoomNumber, int? pricePerNight, bool isEmployee, string note)
        {
            LicensePlate = licensePlate;
            CarModel = carModel;
            GuestsName = guestsName;
            GuestRoomNumber = guestRoomNumber;
            PricePerNight = pricePerNight;
            IsEmployee = isEmployee;
            Note = note;
            NumberOfStays = 0;
        }

        public void Update(Reservation reservation)
        {
            this.LicensePlate = reservation.Car.LicensePlate;
            this.CarModel = reservation.Car.CarModel;
            this.GuestsName = reservation.Car.GuestsName;
            this.GuestRoomNumber = reservation.Car.GuestRoomNumber;
            this.PricePerNight = reservation.Car.PricePerNight;
            this.IsEmployee = reservation.Car.IsEmployee;
            this.Note = reservation.Car.Note;
        }

        public string CalculateTotalPrice(DateTime arrivalDate, DateTime departureDate, int? pricePerNight)
        {
            if (pricePerNight == null)
            {
                return "Nevyplněno";
            }

            if (departureDate.Year > arrivalDate.Year)
            {
                var daysToEndOfYear = (DateTime.IsLeapYear(arrivalDate.Year) ? 366 : 365)-arrivalDate.DayOfYear;

                return ((daysToEndOfYear + departureDate.DayOfYear) * pricePerNight).ToString();
            }

            var numberOfDays = departureDate.DayOfYear - arrivalDate.DayOfYear;

            if (numberOfDays == 0)
            {
                return pricePerNight.ToString();
            }

            return (numberOfDays * pricePerNight).ToString();
        }

        public void AddStay() {
            this.NumberOfStays += 1;
        }

        public void ResetPricePerNightToNull()
        {
            this.PricePerNight = null;
        }
    }
}