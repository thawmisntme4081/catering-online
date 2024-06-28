using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using System;
using backend.Models.DTO;

namespace backend.Controllers
{
    [Authorize(Roles = "Caterer")]
    [Route("api/[controller]")]
    [ApiController]
    public class CatererController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private Item item = new();

        // GET: api/Caterer/items
        [HttpGet("items")]
        public async Task<ActionResult> GetItems()
        {
            var cid = HttpContext.Session.GetInt32("cid");
            if (cid == null)
            {
                return NotFound("Caterer id not found");
            }
            // Get item for current caterer
            var items = await _context.Items
                .Where(i => i.CatererId == cid)
                .Select(i => new {
                    i.Id,
                    i.Name,
                    i.Image,
                    i.ServesCount,
                    i.Price,
                    i.CuisineId,
                    i.CreatedAt,
                    i.UpdatedAt
                })
                .ToListAsync();

            return Ok(items);
        }

        // POST: api/Caterer/id/items
        [HttpPost("items")]
        public async Task<ActionResult> AddItem(ItemDTO request)
        {
            var cid = HttpContext.Session.GetInt32("cid");
            if (cid == null)
            {
                return NotFound("Caterer id not found");
            }
            // Add item
            item.CatererId = cid.Value;
            item.CuisineId = request.CuisineId;
            item.Name =  request.Name;
            item.Price = request.Price;
            item.ServesCount = request.ServesCount;
            item.Image = request.Image;
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return Ok("Item created successfully");
        }

        // PUT: api/Caterer/items/{id}
        [HttpPut("items/{id}")]
        public async Task<ActionResult> UpdateItem(int id, ItemDTO request)
        {
            var cid = HttpContext.Session.GetInt32("cid");
            if (cid == null)
            {
                return NotFound("Caterer id not found");
            }
            // Get item from caterer id and item id
            var existingItem = await _context.Items.Where(x => x.CatererId == cid && x.Id == id).FirstOrDefaultAsync();
            if (existingItem == null)
            {
                return NotFound("Item not found.");
            }
            // Update item
            existingItem.Name = request.Name;
            existingItem.Image = request.Image;
            existingItem.CuisineId = request.CuisineId;
            existingItem.ServesCount = request.ServesCount;
            existingItem.Price = request.Price;
            existingItem.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok("Item updated successfully.");
        }

        // DELETE: api/Caterer/items/{id}
        [HttpDelete("items/{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var cid = HttpContext.Session.GetInt32("cid");
            if (cid == null)
            {
                return NotFound("Caterer id not found");
            }
            // Get item from caterer id and item id
            var item = await _context.Items.Where(x => x.CatererId == cid && x.Id == id).FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound("Item not found.");
            }
            // Delete item
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Item deleted successfully.");
        }

        // GET: api/Caterer/cuisines
        [HttpGet("cuisines")]
        public async Task<ActionResult> GetCuisines()
        {
            var cuisines = await _context.CuisineTypes
                .Select(ct => new {
                    ct.Id,
                    ct.CuisineName
                })
                .ToListAsync();

            return Ok(cuisines);
        }
    }
}
