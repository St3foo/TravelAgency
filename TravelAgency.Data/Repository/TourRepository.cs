using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;

namespace TravelAgency.Data.Repository
{
    public class TourRepository : BaseRepository<Tour, Guid>, ITourRepository
    {
        public TourRepository(TravelAgencyDbContext context) : base(context)
        {
        }
    }
}
