using Microsoft.AspNetCore.Identity;

namespace TravelAgency.Data.Models
{
    public class UserTour
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserId { get; set; } = null!;

        public virtual IdentityUser User { get; set; } = null!;

        public Guid TourId { get; set; }

        public virtual Tour Tour { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
