using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeMugTest.Models;
using System.Data;
using System.Data.SqlClient;
using CoffeeMugTest.Data;

namespace CoffeeMugTest.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDataProvider _dataProvider;

        public ProductController()
        {
            _dataProvider = new ProductDataProvider();
        }
        

        [HttpGet("/api/list")]
        public async Task<IEnumerable<Products>> List()
        {
            return await _dataProvider.GetProducts();            
        }

        [HttpGet("/api/product/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _dataProvider.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut("/api/add")]
        public async Task<IActionResult> AddProduct([FromBody]Products product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            product.Id = Guid.NewGuid();
            try
            {
                 await _dataProvider.AddProduct(product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return Ok(product.Id);
        }

        [HttpPut("/api/update")]
        public async Task<IActionResult> UpdateProduct([FromBody] Products product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _dataProvider.UpdateProduct(product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok($"Product {product.Name} has been updated succesfully.");
        }

        [HttpDelete("/api/delete/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _dataProvider.DeleteProduct(id);

            return Ok($"product {id} has been deleted succesfully");
        }
    }
}