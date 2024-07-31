using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IM.API.Database;
using IM.Library.Models;

namespace IM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopItem>>> Get()
        {
            return await _context.ShopItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShopItem>> Get(int id)
        {
            var item = await _context.ShopItems.FindAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<ShopItem>> Post([FromBody] ShopItem item)
        {
            _context.ShopItems.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ShopItem item)
        {
            if (id != item.Id)
            {
                return BadRequest("Item ID mismatch.");
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ShopItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.ShopItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShopItemExists(int id)
        {
            return _context.ShopItems.Any(e => e.Id == id);
        }
    }
}
