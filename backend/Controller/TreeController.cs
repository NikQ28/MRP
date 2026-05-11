using backend.Domain.Contract;
using backend.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class TreeController(ITreeService treeService) : ControllerBase
    {
        [HttpGet("{itemId:int}")]
        public async Task<ActionResult<TreeNode>> GetTree(int itemId)
        {
            var tree = await treeService.GetTreeNodeByItemId(itemId);
            return Ok(tree);
        }
    }
}
