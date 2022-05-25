using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.Services
{
    public class SqlDbService : IWarehouseDBService
    {
        private readonly IConfiguration _configuration;

        public SqlDbService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CheckIdProduct(int productId)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("DefaultDbConnection")))
            {
                var com = new SqlCommand("SELECT 1 FROM Product WHERE IdProduct = @productId", con);
                com.Parameters.AddWithValue("@productId", productId);
                await con.OpenAsync();
                var result = await com.ExecuteReaderAsync();
                return result.HasRows;
            }
        }



        public async Task<> RegisterProductInWarehouse()
        {

        }

    }
}
