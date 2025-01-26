using DatingAppAPI.Data;
using DatingAppAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Controllers
{
    public class UserController(AppDbContext db) : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await db.Users.ToListAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{Id}")]
        public async Task<ActionResult<AppUser>> GetUser(int Id)
        {
            var user = await db.Users.FindAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
