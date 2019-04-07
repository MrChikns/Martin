﻿using System.Data.Entity;
using HotelGarage.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HotelGarage.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<ParkingPlace> ParkingPlaces { get; set; }
        public DbSet<StateOfPlace> StatesOfPlace { get; set; }
        public DbSet<StateOfReservation> StateOfReservations { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}