using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using backend.Models.DTO;

namespace backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private CuisineType cuisine = new();

        // GET: api/Admin/users
        [HttpGet("users")]
        public async Task<ActionResult> GetUsers()
        {
            // Get all user
            var user = await _context.Profiles
                .Join(_context.Users, profile => profile.UserId, user => user.Id, (profile, user) => new
                {
                    user.Id,
                    user.Type,
                    user.Email,
                    user.CreatedAt,
                    user.UpdatedAt,
                    profile.FirstName,
                    profile.LastName,
                    profile.Address,
                    profile.PhoneNumber
                })
                .OrderBy(user => user.Id)
                .Take(100)
                .ToListAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        // GET: api/Admin/users/id
        [HttpGet("users/{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            // Get a user
            var user = await _context.Profiles
                .Where(x => x.UserId == id)
                .Join(_context.Users, profile => profile.UserId, user => user.Id, (profile, user) => new
                {
                    user.Id,
                    user.Type,
                    user.Email,
                    user.CreatedAt,
                    user.UpdatedAt,
                    profile.FirstName,
                    profile.LastName,
                    profile.Address,
                    profile.PhoneNumber,
                })
                .FirstOrDefaultAsync();
            if (user == null)
            { 
                return NotFound("User not found"); 
            }

            return Ok(user);
        }

        // DELETE: api/Admin/users/id
        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var uid = HttpContext.Session.GetInt32("uid");
            if (uid == null)
            { 
                return NotFound("User id not found"); 
            }
            if (uid == id)
            { 
                return BadRequest("Can't delete current admin account"); 
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            { 
                return NotFound("User not found"); 
            }
            // Delete a user
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User deleted successfully");
        }

        // GET: api/Admin/cuisines
        [HttpGet("cuisines")]
        public async Task<ActionResult> GetCuisines()
        {
            var cuisines = await _context.CuisineTypes
                .Select(ct => new {
                    ct.Id,
                    ct.CuisineName,
                    ct.CreatedAt,
                    ct.UpdatedAt
                })
                .ToListAsync();

            return Ok(cuisines);
        }

        // POST: api/Admin/cuisines
        [HttpPost("cuisines")]
        public async Task<ActionResult> AddCuisine(CuisineDTO req)
        {
            cuisine.CuisineName = req.CuisineName;
            cuisine.CreatedAt = DateTime.UtcNow;
            cuisine.UpdatedAt = DateTime.UtcNow;
            _context.CuisineTypes.Add(cuisine);
            await _context.SaveChangesAsync();

            return Ok("Cuisine created successfully");
        }

        // PUT: api/Admin/cuisines/{id}
        [HttpPut("cuisines/{id}")]
        public async Task<ActionResult> UpdateCuisine(int id, CuisineDTO req)
        {
            var existingCuisine = await _context.CuisineTypes.FindAsync(id);
            if (existingCuisine == null)
            {
                return NotFound("Cuisine type not found.");
            }
            // Update cuisine
            existingCuisine.CuisineName = req.CuisineName;
            existingCuisine.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok("Cuisine type updated successfully.");
        }

        // DELETE: api/Admin/cuisines/{id}
        [HttpDelete("cuisines/{id}")]
        public async Task<ActionResult> DeleteCuisine(int id)
        {
            var cuisine = await _context.CuisineTypes.FindAsync(id);
            if (cuisine == null)
            {
                return NotFound("Cuisine type not found.");
            }
            // Delete cuisine
            _context.CuisineTypes.Remove(cuisine);
            await _context.SaveChangesAsync();

            return Ok("Cuisine type deleted successfully.");
        }

        // GET: api/Admin/items
        [HttpGet("items")]
        public async Task<ActionResult> GetItems(int catererId)
        {
            var caterer = await _context.Caterers.FindAsync(catererId);
            if (caterer == null)
            {
                return NotFound("Caterer not found");
            }
            // Get item from caterer id
            var items = await _context.Items
                .Where(i => i.CatererId == catererId)
                .Select(i => new {
                    i.Id,
                    i.Name,
                    i.Image,
                    i.ServesCount,
                    i.Price,
                    i.CatererId,
                    i.CuisineId,
                    i.CreatedAt,
                    i.UpdatedAt
                })
                .ToListAsync();

            return Ok(items);
        }

        // DELETE: api/Admin/items/{id}
        [HttpDelete("items/{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound("Item not found.");
            }
            // Delete item
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Item deleted successfully.");
        }
    }
}
