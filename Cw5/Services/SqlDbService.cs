using Cw5.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
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

        public async Task<bool> CheckIdProduct(int idProduct)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("DefaultDbConnection")))
            {
                var com = new SqlCommand("SELECT 1 FROM Product WHERE IdProduct = @idProduct", con);
                com.Parameters.AddWithValue("@idProduct", idProduct);
                await con.OpenAsync();
                var result = await com.ExecuteReaderAsync();
                return result.HasRows;
            }
        }


        public async Task<bool> CheckIdWarehouse(int idWarehouse)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("DefaultDbConnection")))
            {
                var com = new SqlCommand("SELECT 1 FROM Warehouse WHERE IdWarehouse = @IdWarehouse", con);
                com.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
                await con.OpenAsync();
                var result = await com.ExecuteReaderAsync();
                return result.HasRows;
            }
        }

        public async Task<bool> CheckData(int id, string tableName, string attributName)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("DefaultDbConnection")))
            {
                var com = new SqlCommand("SELECT 1 FROM @tableName WHERE @attributName = @id", con);
                com.Parameters.AddWithValue("@id", id);
                com.Parameters.AddWithValue("@tableName", tableName);
                com.Parameters.AddWithValue("@attributName", attributName);

                await con.OpenAsync();
                var result = await com.ExecuteReaderAsync();
                return result.HasRows;
            }
        }


        public async Task<int> GetOrderId(RegisterProduct registerProduct)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("DefaultDbConnection")))
            {
                var com = new SqlCommand("SELECT IdOrder FROM Order WHERE idProduct = @idProduct " +
                    "AND Amount = @amount AND CreatedAt < @regCreatedAt", con);
                com.Parameters.AddWithValue("@idProduct", registerProduct.IdProduct);
                com.Parameters.AddWithValue("@amount", registerProduct.Amount);
                com.Parameters.AddWithValue("@regCreatedAt", registerProduct.CreatedAt);
                await con.OpenAsync();
                var result = await com.ExecuteReaderAsync();

                return int.Parse(result["idOrder"].ToString()); ;
            }
        }

        public async void PutOrder(int idOrder, RegisterProduct register)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("DefaultDbConnection")))
            {
                var com = new SqlCommand(
                    $"UPDATE Order " +
                    $"SET FullfilledAt = @regCreatedAt " +
                    $"where idOrder=@idOrder", con);
                com.Parameters.AddWithValue("@regCreatedAt", register.CreatedAt);
                com.Parameters.AddWithValue("@idOrder", idOrder);
                await con.OpenAsync();
                await com.ExecuteNonQueryAsync();
            }

        }


        public async Task<int> AddProductToWarehouse(RegisterProduct registerProduct, int idOrder)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("DefaultDbConnection")))
            {
                await con.OpenAsync();
                var transaction = await con.BeginTransactionAsync();
                SqlCommand com = con.CreateCommand();
                com.Connection = con;
                com.Transaction = transaction as SqlTransaction;
                try
                {
                    com = new SqlCommand(
                       $"INSERT INTO Product_Warehouse (IdWarehouse,IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                       $"output INSERTED.idproductwarehouse " +
                       $"VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Amount * (select price from product where idProduct=@IdProduct), @CreatedAt);"
                       , con);
                    com.Parameters.AddWithValue("@IdWarehouse", registerProduct.IdWareHouse);
                    com.Parameters.AddWithValue("@IdProduct", registerProduct.IdProduct);
                    com.Parameters.AddWithValue("@IdOrder", idOrder);
                    com.Parameters.AddWithValue("@Amount", registerProduct.Amount);
                    com.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    await con.OpenAsync();

                    var primaryKey = int.Parse((await com.ExecuteScalarAsync()).ToString());
                    await transaction.CommitAsync();

                    return primaryKey;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                    try
                    {
                        await transaction.RollbackAsync();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                    return -1;
                }

            }

        }
    }
}
