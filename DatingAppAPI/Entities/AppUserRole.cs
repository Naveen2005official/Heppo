using Microsoft.AspNetCore.Identity;

namespace DatingAppAPI.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser Users { get; set; } = null!;
        public AppRole Roles { get; set; } = null!;
    }
}
