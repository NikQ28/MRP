using backend.Domain.Contract;
using backend.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<OrderObject>>> GetAllOrders() => Ok(await orderService.GetAllOrderObjectsAsync());

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder([FromBody] OrderRequest request)
        {
            return Ok(await orderService.CreateOrderObjectAsync(request));
        }

        [HttpPut("{orderId:int}")]
        public async Task<ActionResult<int>> UpdateOrder(int orderId, [FromBody] OrderRequest request) =>
            Ok(await orderService.UpdateOrderObjectAsync(orderId, request));

        [HttpDelete("{orderId:int}")]
        public async Task<ActionResult<int>> DeleteOrder(int orderId) =>
            Ok(await orderService.DeleteOrderObjectAsync(orderId));

    }
}
