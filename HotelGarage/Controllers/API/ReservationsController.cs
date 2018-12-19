using HotelGarage.Models;
using HotelGarage.Repositories;
using System.Collections.Generic;
using System.Web.Http;

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


        public List<string> GetReservations()
        {
            return _reservationRepository.GetLicensePlates();
        }

    }
}
