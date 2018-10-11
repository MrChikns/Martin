using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelGarage.Controllers
{
    public class CarsController : Controller
    {
        private ApplicationDbContext _context;

        public CarsController()
        {
            _context = new ApplicationDbContext();
        }

        // Cars form
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Car car)
        {
            if(!ModelState.IsValid)
            {
                return View("Create", car);
            }

            _context.Cars.Add(car);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}