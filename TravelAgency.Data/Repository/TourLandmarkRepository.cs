using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;

namespace TravelAgency.Data.Repository
{
    public class TourLandmarkRepository : BaseRepository<TourLandmark, Guid>, ITourLandmarkRepository
    {
        public TourLandmarkRepository(TravelAgencyDbContext context) : base(context)
        {
        }
    }
}
