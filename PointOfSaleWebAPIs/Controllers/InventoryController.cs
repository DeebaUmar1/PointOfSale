using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointOfSale.Data;
using PointOfSale.Services;
using PointOfSale;

namespace PointOfSaleWebAPIs.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly POSDbContext _context;

        public InventoryController(POSDbContext context)
        {
            _context = context;
        }

        [HttpPost("SeedProducts")]
        public IActionResult SeedProducts()
        {
            EFInventory.SeedProducts(_context);
            return Ok("Products seeded");
        }

        [HttpPost("AddProduct")]
        public IActionResult AddProduct(Product product)
        {
            EFInventory.Add(_context, product);
            return Ok("Product added");
        }

        [HttpGet("ViewProducts")]
        public IActionResult ViewProducts()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        [HttpDelete("RemoveProduct/{id}")]
        public IActionResult RemoveProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok("Product removed");
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct(Product product)
        {
            if (product == null || product.Id == 0)
            {
                return BadRequest("Invalid product data");
            }

            var existingProduct = _context.Products.Find(product.Id);
            if (existingProduct == null)
            {
                return NotFound("Product not found");
            }

            existingProduct.name = product.name;
            existingProduct.category = product.category;
            existingProduct.type = product.type;
            existingProduct.quantity = product.quantity;
            existingProduct.price = product.price;

            _context.SaveChanges();
            return Ok("Product updated");
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
