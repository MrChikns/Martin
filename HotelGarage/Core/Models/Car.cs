using System;
using System.ComponentModel.DataAnnotations;

namespace HotelGarage.Core.Models
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

        public Car(Car car)
        {
            LicensePlate = car.LicensePlate;
            CarModel = car.CarModel;
            GuestsName = car.GuestsName;
            GuestRoomNumber = car.GuestRoomNumber;
            PricePerNight = car.PricePerNight;
            IsEmployee = car.IsEmployee;
            Note = car.Note;
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

        public string ReturnCalculatedTotalPriceString(int numberOfDays, int? pricePerNight)
        {
            if (pricePerNight == null)
            {
                return Helpers.Constants.NotFilledOutMessageConstant;
            }

            if (numberOfDays == 0)
            {
                return pricePerNight.ToString();
            }

            return (numberOfDays * pricePerNight).ToString();
        }

        public int CalculateNumberOfDays(DateTime arrivalDate, DateTime departureDate)
        {
            var numberOfDays = (departureDate - arrivalDate).TotalDays;

            if (numberOfDays < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            
            return (int)numberOfDays;
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