using DatingAppAPI.Data;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingAppAPI.Controllers
{
    public class AccountController(AppDbContext db, ITokenService ts) : BaseApiController
    {
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO dto)
        {
            if (await UserExists(dto.Username)) return BadRequest("Username is taken");
            return Ok();
            //var hmac = new HMACSHA512();
            //var user = new AppUser
            //{
            //    UserName = dto.UserName,
            //    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            //    PasswordSalt = hmac.Key
            //};
            //db.Users.Add(user);
            //await db.SaveChangesAsync();
            //return Ok(
            //    new UserDTO 
            //    { 
            //        UserName = dto.UserName, 
            //        Token = ts.CreateToken(user)
            //    }
            //);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO dto)
        {
            var user = await db.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == dto.Username.ToLower());
            if(user == null)
            {
                return Unauthorized("Invalid Username");
            }
            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
            for(int i = 0;i < computedHash.Length;i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid Password");
            }
            return Ok(
                new UserDTO
                {
                    Username = dto.Username,
                    Token = ts.CreateToken(user),
                    PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                }
            );
        }
        private async Task<bool> UserExists(string username)
        {
            return await db.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }
    }
}
