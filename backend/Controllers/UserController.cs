using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using backend.Models.DTO;

namespace backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        // GET: api/User/profile
        [HttpGet("profile")]
        public async Task<ActionResult> GetUserProfile()
        {
            var uid = HttpContext.Session.GetInt32("uid");
            if (uid == null) 
            {
                return NotFound("User id not found");
            }
            // Get current session user detail
            var userProfile = await _context.Profiles
                .Where(x => x.UserId == uid)
                .Join(_context.Users, profile => profile.UserId, user => user.Id, (profile, user) => new
                {
                    profile.FirstName,
                    profile.LastName,
                    profile.Address,
                    profile.PhoneNumber,
                    profile.Image,
                    user.Email
                })
                .FirstOrDefaultAsync();
            if (userProfile == null) 
            { 
                return NotFound("User not found"); 
            }

            return Ok(userProfile);
        }

        // PUT: api/User/update-profile
        [HttpPut("update-profile")]
        public async Task<ActionResult> UserUpdateProfile(UpdateProfile request)
        {
            var uid = HttpContext.Session.GetInt32("uid");
            if (uid == null) 
            {
                return NotFound("User id not found");
            }
            var userProfile = await _context.Profiles.Where(x => x.UserId == uid).FirstOrDefaultAsync();
            var user = await _context.Users.FindAsync(uid);
            if (userProfile == null || user == null) 
            { 
                return NotFound("User not found."); 
            }
            if (ModelState.IsValid == false) 
            { 
                return BadRequest("Invalid input"); 
            }
            // Update profile
            userProfile.FirstName = request.FirstName;
            userProfile.LastName = request.LastName;
            userProfile.PhoneNumber = request.PhoneNumber;
            userProfile.Address = request.Address;
            userProfile.Image = request.Image;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok("Profile updated successfully.");
        }

        // PUT: api/User/change-password
        [HttpPut("change-password")]
        public async Task<ActionResult> UserChangePassword(ChangePassword request)
        {
            var uid = HttpContext.Session.GetInt32("uid");
            if (uid == null) 
            {
                return NotFound("User id not found");
            }
            var user = await _context.Users.FindAsync(uid);
            if (user == null) 
            { 
                return NotFound("User not found."); 
            }
            if (BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password) == false) 
            { 
                return BadRequest("Wrong password"); 
            }
            if (ModelState.IsValid == false) 
            { 
                return BadRequest("Invalid input"); 
            }
            // Change password
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok("Password changed successfully.");
        }
    }
}
