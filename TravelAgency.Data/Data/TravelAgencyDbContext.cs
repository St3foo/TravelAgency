using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TravelAgency.Data
{
    public class TravelAgencyDbContext : IdentityDbContext
    {
        public TravelAgencyDbContext(DbContextOptions<TravelAgencyDbContext> options)
            : base(options)
        {
        }
    }
}
