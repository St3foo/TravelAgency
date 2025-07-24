using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;

namespace TravelAgency.Data.Repository
{
    public class UserHotelRepository : BaseRepository<UserHotel, Guid>, IUserHotelRepository
    {
        public UserHotelRepository(TravelAgencyDbContext context) 
            : base(context)
        {
        }
    }
}
