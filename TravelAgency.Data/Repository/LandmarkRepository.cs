using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;

namespace TravelAgency.Data.Repository
{
    public class LandmarkRepository : BaseRepository<Landmark, Guid>, ILandmarkRepository
    {
        public LandmarkRepository(TravelAgencyDbContext context) 
            : base(context)
        {
        }
    }
}
