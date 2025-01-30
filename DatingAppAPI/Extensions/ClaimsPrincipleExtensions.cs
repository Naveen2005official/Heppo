using System.Security.Claims;

namespace DatingAppAPI.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var username = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("Cannot get username from token");

            return username;
        }
    }
}
