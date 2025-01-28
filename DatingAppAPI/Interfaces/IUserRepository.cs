using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;

namespace DatingAppAPI.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<AppUser?> GetByIdAsync(int id);
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<MemberDTO>> GetMembersAsync();
        Task<MemberDTO?> GetMemberAsync(string username);
    }
}
