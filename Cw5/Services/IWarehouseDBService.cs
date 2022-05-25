using Cw5.Models;
using System.Threading.Tasks;

namespace Cw5.Services
{
    public interface IWarehouseDBService
    {
        public Task<bool> CheckIdProduct(int productId);
        public Task<bool> CheckIdWarehouse(int idWarehouse);
        public Task<int> GetOrderId(RegisterProduct register);
        public Task<bool> CheckData(int id, string tableName, string attributName);
        public void PutOrder(int idOrder, RegisterProduct register);
        public Task<int> AddProductToWarehouse(RegisterProduct registerProduct, int idOrder);

    }



}
