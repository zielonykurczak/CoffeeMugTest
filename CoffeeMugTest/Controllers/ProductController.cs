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
using NLog;
using Microsoft.Extensions.Logging;

namespace CoffeeMugTest.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDataProvider _dataProvider;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _dataProvider = new ProductDataProvider();
            _logger = logger;
        }
        

        [HttpGet("/api/list")]
        public async Task<IEnumerable<Products>> List()
        {
            _logger.LogDebug("Get all products.");

            try
            {
                return await _dataProvider.GetProducts();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured. Check stacktrace below: \n\n" + ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("/api/product/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Products product;
            try
            {
                product = await _dataProvider.GetProduct(id);
                if (product == null)
                {
                    _logger.LogWarning($"No product with guid: {id} in database!");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured. Check stacktrace below: \n\n" + ex.StackTrace);
                throw ex;
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
                _logger.LogError("An error occured. Check stacktrace below: \n\n" + ex.StackTrace);
                throw ex;
            }
            _logger.LogDebug($"New product with guid {product.Id} added succesfully.");
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
                _logger.LogError("An error occured. Check stacktrace below: \n\n" + ex.StackTrace);
                throw ex;
            }
            _logger.LogDebug($"Product {product.Name} with guid: {product.Id} has been updated succesfully.");
            return Ok($"Product {product.Name} has been updated succesfully.");
        }

        [HttpDelete("/api/delete/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _dataProvider.DeleteProduct(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured. Check stacktrace below: \n\n" + ex.StackTrace);
                throw ex;
            }
            _logger.LogDebug($"product {id} has been deleted succesfully");
            return Ok($"product {id} has been deleted succesfully");
        }
    }
}