using HotelGarage.Models;
using HotelGarage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;

namespace HotelGarage.Controllers.API
{
    public class ReservationsController : ApiController
    {
        private ApplicationDbContext _context;
        private readonly ReservationRepository _reservationRepository;

        public ReservationsController()
        {
            _context = new ApplicationDbContext();
            _reservationRepository = new ReservationRepository(_context);
        }

        public IEnumerable<Reservation> GetReservations()
        {
            return _reservationRepository.GetReturningReservationsCars();
        }

    }
}
