using IM.API.EC;
using IM.Library.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryEC _inventoryEC;

        public InventoryController()
        {
            _inventoryEC = new InventoryEC();
        }

        [HttpGet]
        public ActionResult<IEnumerable<ShopItemDTO>> GetAllItems()
        {
            var items = _inventoryEC.GetAllItems();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<ShopItemDTO> GetItemById(int id)
        {
            var item = _inventoryEC.GetItemById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<ShopItemDTO> AddItem([FromBody] ShopItemDTO newItem)
        {
            var addedItem = _inventoryEC.AddItem(newItem);
            return CreatedAtAction(nameof(GetItemById), new { id = addedItem.Id }, addedItem);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateItem(int id, [FromBody] ShopItemDTO updatedItem)
        {
            var result = _inventoryEC.UpdateItem(id, updatedItem);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(int id)
        {
            var result = _inventoryEC.DeleteItem(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
