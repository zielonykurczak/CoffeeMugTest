using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CoffeeMugTest.Models;
using Dapper;

namespace CoffeeMugTest.Data
{
    public class ProductDataProvider : IProductDataProvider
    {
        private readonly string connectionString = "Server=LAPTOP-3U7H8EPE;Database=Test;Integrated Security=True";
        public async Task AddProduct(Products product)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                string query = $"INSERT into Products values ('{product.Id}','{product.Name}','{product.Price}')";

                try
                {
                    await sqlConnection.OpenAsync();
                    await sqlConnection.ExecuteAsync(query);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task DeleteProduct(Guid Id)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                string query = $"DELETE from Products WHERE Id = '{Id}'";

                try
                {
                    await sqlConnection.OpenAsync();
                    var result = await sqlConnection.ExecuteAsync(query);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<Products> GetProduct(Guid Id)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                string query = $"SELECT * from Products WHERE Id = '{Id}'";
                Products result;
                try
                {
                    await sqlConnection.OpenAsync();
                    result = await sqlConnection.QuerySingleOrDefaultAsync<Products>(query);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            }
        }

        public async Task<IEnumerable<Products>> GetProducts()
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var query = "SELECT * from Products";
                IEnumerable<Products> result;
                try
                {
                    await sqlConnection.OpenAsync();
                    result = await sqlConnection.QueryAsync<Products>(query);
                }
                catch (Exception)
                {
                    return null;
                    throw;
                }

                return result;
            }
        }

        public async Task UpdateProduct(Products product)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var isExisting = GetProduct(product.Id);
                
                if (isExisting.Result!=null)
                {
                    string query = $"UPDATE Products set Name = '{product.Name}', Price = '{product.Price}' WHERE Id = '{product.Id}'";

                    try
                    {
                        await sqlConnection.OpenAsync();
                        await sqlConnection.ExecuteAsync(query);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        
    }
}
