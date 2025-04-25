using Microsoft.AspNetCore.Identity;

namespace DatingAppAPI.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}
