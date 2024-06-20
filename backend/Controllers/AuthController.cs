using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ApplicationDbContext context) : ControllerBase {
        private readonly ApplicationDbContext _context = context;
        private readonly User user = new();
        private readonly UserProfile userProfile = new();

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegister request) {
            if (_context.Users.Any(x => x.Email == request.Email)) {
                return BadRequest("Email already exist");
            }
            if (ModelState.IsValid == false) {
                return BadRequest("Invalid input");
            }
            // Add user to Users table
            user.Type = request.Type;
            user.Email = request.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            // Add user profile to UserProfile table
            userProfile.UserID = user.ID;
            userProfile.FirstName = request.FirstName;
            userProfile.LastName = request.LastName;
            userProfile.Address = request.Address;
            userProfile.PhoneNumber = request.PhoneNumber;
            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully");
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLogin request) {
            var getUser = await _context.Users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();
            if (getUser == null) {
                return NotFound("Email not found");
            }
            if (BCrypt.Net.BCrypt.Verify(request.Password, getUser.Password) == false) {
                return BadRequest("Wrong password");
            }
            if (ModelState.IsValid == false) {
                return BadRequest("Invalid input");
            }
            // Create session
            HttpContext.Session.SetString("sid", HttpContext.Session.Id);
            HttpContext.Session.SetInt32("uid", getUser.ID);
            await HttpContext.Session.CommitAsync();
            // Create authentication cookie
            var claims = new List<Claim> {
                    new("UID", getUser.ID.ToString()),
                    new(ClaimTypes.Role, getUser.Type.ToString()),
                };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties {
                IsPersistent = false           
            };
            // Create an encrypted cookie and adds it to the current response.
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Ok(getUser.Type);
        }

        // POST: api/Auth/logout
        [HttpPost("logout")]
        public async Task<ActionResult> Logout() {
            // Clear authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Clear session
            HttpContext.Session.Remove("sid");
            HttpContext.Session.Remove("uid");
            await HttpContext.Session.CommitAsync();
            return Ok("Logged out successfully");
        }
    }
}
