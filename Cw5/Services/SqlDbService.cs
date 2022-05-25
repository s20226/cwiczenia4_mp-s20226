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

        public async Task<bool> CheckProductWarehouse(int idOrder)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("DefaultDbConnection")))
            {
                var com = new SqlCommand("SELECT 1 FROM Product_Warehouse WHERE idorder = @idOrder", con);
                com.Parameters.AddWithValue("@idOrder", idOrder);
                await con.OpenAsync();
                var result = await com.ExecuteReaderAsync();
                return result.HasRows;
            }
        }


        public async Task<int> GetOrderId(RegisterProduct registerProduct)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("DefaultDbConnection")))
            {
                var com = new SqlCommand("SELECT idorder FROM [Order] WHERE idProduct = @idProduct " +
                    "AND Amount = @amount AND CreatedAt < @regCreatedAt", con);
                com.Parameters.AddWithValue("@idProduct", registerProduct.IdProduct);
                com.Parameters.AddWithValue("@amount", registerProduct.Amount);
                com.Parameters.AddWithValue("@regCreatedAt", registerProduct.CreatedAt.ToString("yyyy-M-d"));
                await con.OpenAsync();

                using (var result = await com.ExecuteReaderAsync())
                {

                    while (await result.ReadAsync())
                    {
                        return int.Parse(result["idOrder"].ToString());
                    }
                }
                return 0;
            }
        }

        public async void PutOrder(int idOrder, RegisterProduct register)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("DefaultDbConnection")))
            {
                var com = new SqlCommand(
                    $"UPDATE [Order] " +
                    $"SET FulfilledAt = @regCreatedAt " +
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
                SqlCommand command = con.CreateCommand();
                command.Connection = con;
                command.Transaction = transaction as SqlTransaction;
                try
                {
                    double price = 0;
                    command.CommandText = "SELECT price FROM Product WHERE IdProduct = @IdOrder";
                    command.Parameters.AddWithValue("@IdOrder", idOrder);
                    using(var dr = await command.ExecuteReaderAsync())
                    {

                        while (await dr.ReadAsync())
                        {
                            price =double.Parse(dr["price"].ToString());
                        }
                    }
                    command.Parameters.Clear();

                    command.CommandText = "INSERT INTO Product_Warehouse (idwarehouse, idproduct, idorder, amount, price, createdat) output INSERTED.idproductwarehouse VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @price, GETDATE())";
                    command.Parameters.AddWithValue("@IdWarehouse", registerProduct.IdWareHouse);
                    command.Parameters.AddWithValue("@IdProduct", registerProduct.IdProduct);
                    command.Parameters.AddWithValue("@IdOrder", idOrder);
                    command.Parameters.AddWithValue("@Amount", registerProduct.Amount);
         
                    command.Parameters.AddWithValue("@price", price * registerProduct.Amount);


                    var primaryKey = int.Parse((await command.ExecuteScalarAsync()).ToString());
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