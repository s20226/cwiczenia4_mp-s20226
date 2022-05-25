using Cw5.Models;
using System.Threading.Tasks;

namespace Cw5.Services
{
    public interface IWarehouseDBService
    {
        public Task<bool> CheckIdProduct(int productId);
        public Task<bool> CheckIdWarehouse(int idWarehouse);
        public Task<int> GetOrderId(RegisterProduct register);

        public void PutOrder(int idOrder, RegisterProduct register);
        public Task<int> AddProductToWarehouse(RegisterProduct registerProduct, int idOrder);
        public Task<bool> CheckProductWarehouse(int id);
        public Task<int> AddProductToWarehouse(RegisterProduct registerProduct);

    }



}
