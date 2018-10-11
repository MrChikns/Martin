﻿using System.ComponentModel.DataAnnotations;

namespace HotelGarage.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        public string LicensePlate { get; set; }

        [StringLength(50)]
        public string CarModel { get; set; }

        [StringLength(50)]
        public string GuestsName { get; set; }
        
        public int GuestRoomNumber { get; set; }

        public  int PricePerNight { get; set; }
        
        [Required]
        public bool IsEmployee { get; set; }


    }
}