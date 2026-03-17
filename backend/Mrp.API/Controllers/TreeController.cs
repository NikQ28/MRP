using Microsoft.AspNetCore.Mvc;
using Mrp.Core.Abstractions;
using Mrp.Core.Models.DTO;

namespace Mrp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TreeController(ITreeService treeService) : ControllerBase
    {
        [HttpGet("{rootId:int}")]
        public async Task<ActionResult<ItemNode>> GetTree(int rootId)
        {
            var tree = await treeService.GetTree(rootId);
            if (tree is null)
                return NotFound();

            return Ok(tree);
        }
    }
}