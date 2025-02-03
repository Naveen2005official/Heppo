using AutoMapper;
using DatingAppAPI.Data;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Interfaces;
using DatingAppAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingAppAPI.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, ITokenService ts, IMapper mapper) : BaseApiController
    {
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO dto)
        {
            if (await UserExists(dto.Username)) return BadRequest("Username is taken");

            var user = mapper.Map<AppUser>(dto);

            user.UserName = dto.Username.ToLower();

            var result = await userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return new UserDTO
            {
                Username = user.UserName,
                Token = await ts.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO dto)
        {
            var user = await userManager.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.NormalizedUserName == dto.Username.ToUpper());
            if(user == null || user.UserName == null)
            {
                return Unauthorized("Invalid Username");
            }

            var result = await userManager.CheckPasswordAsync(user, dto.Password);
            if (!result) return Unauthorized();
            
            return Ok(
                new UserDTO
                {
                    Username = user.UserName,
                    KnownAs = user.KnownAs,
                    Token = await ts.CreateToken(user),
                    Gender = user.Gender,
                    PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                }
            );
        }
        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());
        }
    }
}
