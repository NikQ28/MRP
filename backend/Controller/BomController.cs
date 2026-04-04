using backend.Domain.Contract;
using backend.Domain.DTO;
using backend.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class BomController(IBomService bomService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<BomResponse>>> GetAllBoms()
        {
            var watchBoms = await bomService.GetAllBoms();
            var response = watchBoms.Select(w => new BomResponse(w.Id, w.ParentId, w.ComponentId, w.Count));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateBom([FromBody] BomRequest request)
        {
            var bom = new Bom
            {
                Id = 0,
                ParentId = request.ParentId,
                ComponentId = request.ComponentId,
                Count = request.Count
            };
            return Ok(await bomService.CreateBom(bom));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> UpdateBom(int id, [FromBody] BomRequest request) =>
            Ok(await bomService.UpdateBom(id, request.ParentId, request.ComponentId, request.Count));

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> DeleteBom(int id) => 
            Ok(await bomService.DeleteBom(id));
    }
}
