using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using backend.Models.DTO;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private User user = new();
        private Profile profile = new();
        private Caterer caterer = new();

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<ActionResult> Register(Register request)
        {
            if (_context.Users.Any(x => x.Email == request.Email))
            { 
                return BadRequest("Email already exist"); 
            }
            if (ModelState.IsValid == false) 
            { 
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

            // Add user profile to Profiles table
            profile.UserId = user.Id;
            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.Address = request.Address;
            profile.PhoneNumber = request.PhoneNumber;
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            // Add caterer to Caterers table
            if (request.Type == Models.User.UserType.Caterer)
            {
                caterer.ProfileId = profile.Id;
                _context.Caterers.Add(caterer);
                await _context.SaveChangesAsync();
            }

            return Ok("User registered successfully");
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(Login request)
        {
            var getUser = await _context.Users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();
            if (getUser == null)
            { 
                return NotFound("Email not found"); 
            }
            var getProfile = await _context.Profiles.Where(x => x.UserId == getUser.Id).FirstOrDefaultAsync();
            if (getProfile == null)
            {
                return NotFound("Profile not found");
            }
            if (BCrypt.Net.BCrypt.Verify(request.Password, getUser.Password) == false) 
            { 
                return BadRequest("Wrong password"); 
            }
            if (ModelState.IsValid == false)
            { 
                return BadRequest("Invalid input"); 
            }
            // Create session
            HttpContext.Session.SetString("sid", HttpContext.Session.Id);
            HttpContext.Session.SetInt32("uid", getUser.Id);
            HttpContext.Session.SetInt32("pid", getProfile.Id);
            if (getUser.Type == Models.User.UserType.Caterer)
            {
                var getCaterer = await _context.Caterers.Where(x => x.ProfileId == getProfile.Id).FirstOrDefaultAsync();
                if (getCaterer == null)
                {
                    return NotFound("Caterer not found");
                }
                HttpContext.Session.SetInt32("cid", getCaterer.Id);
            }
            await HttpContext.Session.CommitAsync();

            // Create authentication cookie
            var claims = new List<Claim> {
                    new("userId", getUser.Id.ToString()),
                    new(ClaimTypes.Role, getUser.Type.ToString()),
                };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false
            };
            // Create encrypted cookie and adds it to the current response.
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok(getUser.Type);
        }

        // POST: api/Auth/logout
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            // Clear authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Clear session
            HttpContext.Session.Remove("sid");
            HttpContext.Session.Remove("uid");
            HttpContext.Session.Remove("pid");
            HttpContext.Session.Remove("cid");
            await HttpContext.Session.CommitAsync();

            return Ok("Logged out successfully");
        }
    }
}
