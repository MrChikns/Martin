using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelGarage.Controllers
{
    public class ReservationsController : Controller
    {
        private ApplicationDbContext _context;

        public ReservationsController()
        {
            _context = new ApplicationDbContext();
        }

        // Cars form
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", reservation);
            }

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}