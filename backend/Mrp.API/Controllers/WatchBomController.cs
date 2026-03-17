using Microsoft.AspNetCore.Mvc;
using Mrp.API.Contracts;
using Mrp.Core.Abstractions;
using Mrp.Core.Models;

namespace Mrp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WatchBomController(IWatchBomsService watchBomsService) : ControllerBase
    {
        private readonly IWatchBomsService _watchBomsService = watchBomsService;

        [HttpGet]
        public async Task<ActionResult<List<WatchBomsResponse>>> GetAllWatchBoms()
        {
            var watchBoms = await _watchBomsService.GetAllWatchBoms();
            var response = watchBoms.Select(w => new WatchBomsResponse(w.Id, w.ParentId, w.ChildId, w.Count));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateWatchBom([FromBody] WatchBomsRequest request)
        {
            var (watchBom, error) = WatchBom.Create(0, request.ParentId, request.ChildId, request.Count);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            return Ok(await _watchBomsService.CreateWatchBom(watchBom));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> UpdateWatchBom(int id, [FromBody] WatchBomsRequest request)
        {
            return Ok(await _watchBomsService.UpdateWatchBom(id, request.ParentId, request.ChildId, request.Count));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> DeleteWatchBom(int id)
        {
            return Ok(await _watchBomsService.DeleteWatchBom(id));
        }
    }
}
