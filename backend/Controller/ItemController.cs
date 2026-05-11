using backend.Domain.Contract;
using backend.Domain.DTO;
using backend.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace backend.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController(IItemService itemService, ITreeService treeService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ItemResponse>>> GetItems()
        {
            var items = await itemService.GetAllItems();
            var responses = items.Select(i => new ItemResponse(i.Id, i.Name, i.Description ?? string.Empty));
            return Ok(responses);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateItem([FromBody] ItemRequest request)
        {
            var item = new Item
            {
                Id = 0,
                Name = request.Name,
                Description = request.Description
            };
            return Ok(await itemService.CreateItem(item));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> UpdateItem(int id, [FromBody] ItemRequest request) => 
            Ok(await itemService.UpdateItem(id, request.Name, request.Description));

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> DeleteBranch(int id) =>
            Ok(await treeService.DeleteBranchFromNodeId(id));
    }
}
