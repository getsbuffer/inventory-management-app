using Microsoft.AspNetCore.Mvc;
using IM.Library.DTO;

namespace IM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private static List<ShopItemDTO> _items = new List<ShopItemDTO>();

        [HttpGet]
        public ActionResult<IEnumerable<ShopItemDTO>> Get()
        {
            return Ok(_items);
        }

        [HttpGet("{id}")]
        public ActionResult<ShopItemDTO> Get(int id)
        {
            var item = _items.Find(i => i.Id == id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public ActionResult Post([FromBody] ShopItemDTO item)
        {
            _items.Add(item);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ShopItemDTO item)
        {
            if (id != item.Id)
            {
                return BadRequest("Item ID mismatch.");
            }

            var existingItem = _items.Find(i => i.Id == id);
            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = item.Name;
            existingItem.Desc = item.Desc;
            existingItem.Price = item.Price;
            existingItem.Amount = item.Amount;
            existingItem.IsBogo = item.IsBogo;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var item = _items.Find(i => i.Id == id);
            if (item == null) return NotFound();

            _items.Remove(item);
            return Ok();
        }
    }
}
