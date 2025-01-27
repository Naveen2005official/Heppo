using DatingAppAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DatingAppAPI.Data
{
    public class Seed
    {
        public static async Task SeedUsers(AppDbContext db)
        {
            if (await db.Users.AnyAsync()) return;
            var userData = await File.ReadAllTextAsync("Data/UserDataSeed.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);
            if(users == null) return;
            foreach(var user in users)
            {
                var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;
                db.Users.Add(user);
            }
            await db.SaveChangesAsync();
        }
    }
}
