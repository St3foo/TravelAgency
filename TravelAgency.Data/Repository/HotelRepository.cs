using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;

namespace TravelAgency.Data.Repository
{
    public class HotelRepository : BaseRepository<Hotel, Guid>, IHotelRepository
    {
        public HotelRepository(TravelAgencyDbContext context) 
            : base(context)
        {
        }
    }
}
