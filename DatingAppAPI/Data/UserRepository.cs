using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Helpers;
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

        public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams)
        {
            var query = db.Users.AsQueryable();
            query = query.Where(x => x.UserName != userParams.CurrentUserName);
            if(userParams.Gender != null)
            {
                query = query.Where(x => x.Gender ==  userParams.Gender);
            }

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(x => x.LastActive)
            };
            return await PagedList<MemberDTO>.CreateAsync(query.ProjectTo<MemberDTO>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
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
