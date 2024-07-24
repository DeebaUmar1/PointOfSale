using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointOfSale.Data;
using PointOfSale.Services;
using PointOfSale;
using Microsoft.AspNetCore.Authorization;

namespace PointOfSaleWebAPIs.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly POSDbContext _context;

        public InventoryController(POSDbContext context, ILogger<InventoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("SeedProducts")]
        public IActionResult SeedProducts()
        {
            try
            {
                EFInventory.SeedProducts(_context);
                _logger.LogInformation("Products added!!");
                return Ok("Products seeded");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPost("AddProduct")]
        public IActionResult AddProduct(Product product)
        {
            try
            {
                bool added = EFInventory.AddProductAPI(_context, product);
                if (added)
                {
                    _logger.LogInformation($"Product added!!{product}");
                    return Ok("Product added");
                }
                else
                {
                    _logger.LogWarning("All fields are required");
                    return BadRequest("All fields are required");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("ViewProducts")]
        public IActionResult ViewProducts()
        {
            try
            {
                var products = _context.Products.ToList();
                _logger.LogInformation($"{products.Count} products");
                return Ok(products);
            }
            catch(Exception ex) 
            {
                _logger.LogError($"Error Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
         
        }

        [HttpDelete("RemoveProduct/{id}")]
        public IActionResult RemoveProduct(int id)
        {
            try
            {
                EFInventory.RemoveProduct(_context, id);
                _logger.LogInformation($"Product with {id} is removed");
                return Ok("Product removed");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct(Product product)
        {
            try
            {
                bool updated = EFInventory.UpdateAPI(_context, product);

                if (updated)
                {
                    _logger.LogInformation("Product has been updated!");
                    return Ok("Product updated");
                }
                else
                {
                    _logger.LogWarning("Product has not been updated");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        /* [HttpPut("UpdateStock/{id}")]
         public IActionResult UpdateStock(int id, [FromQuery] string option, [FromQuery] int quantity)
         {
             var product = _context.Products.Find(id);
             if (product == null)
             {
                 return NotFound("Product not found");
             }

             if (option == "increment")
             {
                 product.quantity += quantity;
             }
             else if (option == "decrement")
             {
                 product.quantity -= quantity;
             }
             else
             {
                 return BadRequest("Invalid option");
             }

             _context.SaveChanges();
             return Ok("Product stock updated");
         }*/
    }

}
