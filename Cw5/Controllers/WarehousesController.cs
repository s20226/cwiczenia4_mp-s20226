using Cw5.Models;
using Cw5.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.Controllers
{
    [Route("api/[controller]")]
    [Route("api/warehouses")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseDBService _dbService;

        public WarehousesController(IWarehouseDBService dbService)
        {
            _dbService = dbService;
        }



    [HttpPost]
    public async Task<int> RegisterProductInWarehouse(RegisterProduct register)
        {


        }
    }
}
