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
            bool added = EFInventory.AddProductAPI(_context, product);
            if (added)
            {
                return Ok("Product added");
            }
            else
            {
                return BadRequest("All fields are required");
            }
            
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
            EFInventory.RemoveProduct(_context, id);
            return Ok("Product removed");
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct(Product product)
        {
            bool updated = EFInventory.UpdateAPI(_context,product);

            if (updated)
            {

                return Ok("Product updated");
            }
            else
            {
                return BadRequest();
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
