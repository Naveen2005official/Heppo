using DatingAppAPI.Entities;
using DatingAppAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Data
{
    public class UserRepository(AppDbContext db) : IUserRepository
    {
        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await db.Users.Include(x => x.Photos).ToListAsync();    
        }

        public async Task<AppUser?> GetByIdAsync(int id)
        {
            return await db.Users.FindAsync(id);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await db.Users.Include(x => x.Photos).SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            db.Entry(user).State = EntityState.Modified;
        }
    }
}
