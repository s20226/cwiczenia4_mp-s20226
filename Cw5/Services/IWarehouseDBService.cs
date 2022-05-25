using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.Services
{
    public interface IWarehouseDBService
    {
        public Task<bool> CheckIdProduct(int productId);
    }

    

}
