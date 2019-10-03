using CoffeeMugTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeMugTest.Data
{
    public interface IProductDataProvider
    {
        Task<IEnumerable<Products>> GetProducts();

        Task<Products> GetProduct(Guid Id);

        Task AddProduct(Products product);

        Task UpdateProduct(Products product);

        Task DeleteProduct(Guid Id);
    }
}
