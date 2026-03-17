using Microsoft.AspNetCore.Mvc;
using Mrp.API.Contracts;
using Mrp.Core.Abstractions;
using Mrp.Core.Models;

namespace Mrp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController(IItemsService itemsService) : ControllerBase
    {
        private readonly IItemsService _itemsService = itemsService;

        [HttpGet]
        public async Task<ActionResult<List<ItemsResponse>>> GetItems()
        {
            var items = await _itemsService.GetAllItems();
            var response = items.Select(i => new ItemsResponse(i.Id, i.Name, i.Description));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateItem([FromBody] ItemsRequest request)
        {
            var (item, error) = Item.Create(0, request.Name, request.Description);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            return Ok(await _itemsService.CreateItem(item));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> UpdateItem(int id, [FromBody] ItemsRequest request)
        {
            return Ok(await _itemsService.UpdateItem(id, request.Name, request.Description));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> DeleteItem(int id)
        {
            return Ok(await _itemsService.DeleteItem(id));
        }
    }
}
