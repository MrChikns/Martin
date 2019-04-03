using HotelGarage.Models;
using System.Linq;

namespace HotelGarage.Repositories
{
    public class StateOfPlaceRepository : IStateOfPlaceRepository
    {
        private ApplicationDbContext _context;

        public StateOfPlaceRepository(ApplicationDbContext context)
        {
            _context = context;
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