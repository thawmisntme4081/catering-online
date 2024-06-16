﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserDbContext context) : ControllerBase {
        private readonly UserDbContext _context = context;
        private readonly User user = new();
        private readonly UserProfile userProfile = new();

        [HttpPost("register")]
        public async Task<ActionResult<UserRegister>> Register(UserRegister request) {
            // Validate
            if (_context.Users.Any(x => x.Email == request.Email)) {
                return BadRequest("Email already exist");
            }
            if (ModelState.IsValid == false) {
                return BadRequest("Invalid register detail");
            }
            // Add user
            user.Email = request.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.Users.AddAsync(user);
            // Add user profile
            userProfile.UserID = user.ID;
            userProfile.FirstName = request.FirstName;
            userProfile.LastName = request.LastName;
            userProfile.Address = request.Address;
            userProfile.PhoneNumber = request.PhoneNumber;
            await _context.UserProfiles.AddAsync(userProfile);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserLogin>> Login(UserLogin request) {
            var getUser = await _context.Users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();
            // Validate
            if (getUser == null) {
                return BadRequest("Email not found");
            }
            if (BCrypt.Net.BCrypt.Verify(request.Password, getUser.Password) == false) {
                return BadRequest("Wrong password");
            }
            if (ModelState.IsValid == false) {
                return BadRequest("Invalid login detail");
            }
            var claims = new List<Claim> {
                    new ("UID", getUser.ID.ToString()),
                    new (ClaimTypes.Role, getUser.Type.ToString()),
                };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok("Logged in successfully");
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logged out successfully");
        }
    }
}
