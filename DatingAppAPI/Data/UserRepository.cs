using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Data
{
    public class UserRepository(AppDbContext db, IMapper mapper) : IUserRepository
    {
        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await db.Users.Include(x => x.Photos).ToListAsync();    
        }

        public async Task<AppUser?> GetByIdAsync(int id)
        {
            return await db.Users.FindAsync(id);
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await db.Users
                .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<MemberDTO?> GetMemberAsync(string username)
        {
            return await db.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
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
