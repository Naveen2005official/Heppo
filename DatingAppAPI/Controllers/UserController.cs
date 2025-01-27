using AutoMapper;
using DatingAppAPI.Data;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Controllers
{
    [Authorize]
    public class UserController(IUserRepository userRepository, IMapper mapper) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var users = await userRepository.GetAllAsync();
            var usersToReturn = mapper.Map<IEnumerable<MemberDTO>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            var user = await userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            return mapper.Map<MemberDTO>(user);
        }
    }
}
