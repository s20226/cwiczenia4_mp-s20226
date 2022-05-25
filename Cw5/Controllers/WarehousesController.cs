using Cw5.Models;
using Cw5.Services;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> AddProductToWarehouse(RegisterProduct register)
        {
            //1
            var isIdProduct = await _dbService.CheckIdProduct(register.IdProduct);
            var isIdWarehouse = await _dbService.CheckIdWarehouse(register.IdWareHouse);
            //1a
            if (!isIdProduct && !isIdWarehouse) return NotFound($"{register.IdProduct} and {register.IdWareHouse} not found");

            if (!isIdProduct)
            {
                return NotFound($"{register.IdProduct} not found");
            }
            if (!isIdWarehouse) { return NotFound($"{register.IdWareHouse} not found"); }
            //2
            int idOrder = await _dbService.GetOrderId(register);
            //2a
            if (idOrder == 0)
            {
                return NotFound("Invalid parameter: There is no order to fullfill");
            }
            //3a
            if (await _dbService.CheckData(idOrder, "Product_Warehouse", "idOrder"))
            {
                return BadRequest($"{idOrder} is already fullfil");
            }
            _dbService.PutOrder(idOrder, register);

            var pk = await _dbService.AddProductToWarehouse(register, idOrder);


            return Ok(pk);


        }
    }
}
/* 
 Produkt możemy dodać do hurtowni tylko jeśli w tabeli Order istnieje zlecenie zakupu produktu. 
Sprawdzamy zatem czy w tabeli Order istnieje rekord z: IdProduct i Amount zgodnym z naszym żądaniem. 
CreatedAt zamówienia powinno być mniejsze niż CreatedAt pochodzące z naszego żądania (zamówienie/order powinno pojawić się w bazie danych wcześniej niż nasze żądanie).
 */