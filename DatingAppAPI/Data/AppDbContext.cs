using DatingAppAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }

        public DbSet<AppUser> Users { get; set; }
    }
}
