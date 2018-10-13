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

        [Display(Name = "Číslo Pokoje")]
        public int? GuestRoomNumber { get; set; }

        [Display(Name = "Cena")]
        public int? PricePerNight { get; set; }
        
        public bool IsEmployee { get; set; }


    }
}