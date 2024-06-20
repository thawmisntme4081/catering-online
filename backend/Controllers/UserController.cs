using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using backend.Models.Users;

namespace backend.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ApplicationDbContext context) : ControllerBase {
        private readonly ApplicationDbContext _context = context;

        // GET: api/User/profile
        [HttpGet("profile")]
        public async Task<ActionResult> GetUserProfile() {
            var uid = HttpContext.Session.GetInt32("uid");
            if (uid == null) {
                return Unauthorized("You must log in");
            }
            // Get current user detail
            var userProfile = await _context.UserProfiles
                                            .Where(x => x.UserID == uid)
                                            .Join(_context.Users,
                                            o => o.UserID,
                                            i => i.ID,
                                            (o, i) => new {
                                                o.FirstName,
                                                o.LastName,
                                                o.Address,
                                                o.PhoneNumber,
                                                o.Image,
                                                i.Email
                                            })
                                            .FirstOrDefaultAsync();
            if (userProfile == null) {
                return NotFound("User not found");
            }
            return Ok(userProfile);
        }

        // PUT: api/User/update-profile
        [HttpPut("update-profile")]
        public async Task<ActionResult> UserUpdateProfile(UserUpdateProfile request) {
            var uid = HttpContext.Session.GetInt32("uid");
            if (uid == null) {
                return Unauthorized("You must log in");
            }
            var userProfile = await _context.UserProfiles.FindAsync(uid);
            var user = await _context.Users.FindAsync(uid);
            if (userProfile == null || user == null) {
                return NotFound("User not found.");
            }
            if (ModelState.IsValid == false) {
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
        public async Task<ActionResult> UserChangePassword(UserChangePassword request) {
            var uid = HttpContext.Session.GetInt32("uid");
            if (uid == null) {
                return Unauthorized("You must log in");
            }
            var user = await _context.Users.FindAsync(uid);
            if (user == null) {
                return NotFound("User not found.");
            }
            if (BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password) == false) {
                return BadRequest("Wrong password");
            }
            if (ModelState.IsValid == false) {
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
