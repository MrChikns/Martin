using HotelGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelGarage.Repositories
{
    public class StateOfPlaceRepository
    {
        private ApplicationDbContext _context;

        public StateOfPlaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<StateOfPlace> GetStatesOfPlace()
        {
            return _context.StatesOfPlace.ToList();
        }

        public StateOfPlace GetFreeStateOfPlace()
        {
            return _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Free);
        }

        public StateOfPlace GetReservedStateOfPlace()
        {
            return _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Reserved);
        }

        public StateOfPlace GetOccupiedStateOfPlace()
        {
            return _context.StatesOfPlace.First(s => s.Id == StateOfPlace.Occupied);
        }
    }
}