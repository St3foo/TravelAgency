using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;

namespace TravelAgency.Data.Repository
{
    public class DestinationRepository : BaseRepository<Destination, Guid>, IDestinationRepository
    {
        public DestinationRepository(TravelAgencyDbContext context) 
            : base(context)
        {
        }
    }
}
