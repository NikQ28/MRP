using backend.Domain.Contract;
using backend.Domain.DTO;
using backend.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class StockController(IStockService stockService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<StockResponse>>> GetAllBoms()
        {
            var watchBoms = await stockService.GetAllStocks();
            var responses = watchBoms.Select(s => new StockResponse(s.Id, s.ItemId, s.Count, s.Operation, s.Datetime));
            return Ok(responses);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateBom([FromBody] StockRequest request)
        {
            var bom = new Stock
            {
                Id = 0,
                ItemId = request.ItemId,
                Count = request.Count,
                Operation = request.Operation,
                Datetime = request.Datetime
            };
            return Ok(await stockService.CreateStock(bom));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> UpdateStock(int id, [FromBody] StockRequest request) =>
            Ok(await stockService.UpdateStock(id, request.ItemId, request.Count, request.Operation, request.Datetime));

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> DeleteStock(int id) =>
            Ok(await stockService.DeleteStock(id));
    }
}
