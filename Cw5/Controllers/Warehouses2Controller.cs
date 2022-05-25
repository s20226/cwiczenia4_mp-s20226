using Cw5.Models;
using Cw5.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cw5.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/warehouses2")]
    [ApiController]
    public class Warehouses2Controller : ControllerBase
    {

            private readonly IWarehouseDBService _dbService;

        public Warehouses2Controller(IWarehouseDBService dbService)
        {
            _dbService = dbService;
        }



        [HttpPost]
        public async Task<IActionResult> AddProductToWarehouse(RegisterProduct register)
        {

            var pk = await _dbService.AddProductToWarehouse(register);
            return Ok(pk);
        }
    }
}

