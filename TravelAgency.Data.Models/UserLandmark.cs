using Microsoft.AspNetCore.Identity;

namespace TravelAgency.Data.Models
{
    public class UserLandmark
    {
        public string UserId { get; set; } = null!;

        public virtual IdentityUser User { get; set; } = null!;

        public Guid LandmarkId { get; set; }

        public virtual Landmark Landmark { get; set; } = null!;
    }
}
