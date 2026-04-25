using backend.Domain.Contract;
using backend.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class CalculateController(ICalculateService calculateService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<RequiredItem>>> CalculateOrders() => 
            Ok(await calculateService.CalculateRequiredItems());
    }
}
