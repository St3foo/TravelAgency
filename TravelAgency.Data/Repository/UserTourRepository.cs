using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;

namespace TravelAgency.Data.Repository
{
    public class UserTourRepository : BaseRepository<UserTour, Guid>, IUserTourRepository
    {
        public UserTourRepository(TravelAgencyDbContext context) : base(context)
        {
        }
    }
}
