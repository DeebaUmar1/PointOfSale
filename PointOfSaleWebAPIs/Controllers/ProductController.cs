using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSale;
using PointOfSale.Data;
using PointOfSale.Services;
using System.Net.Http.Headers;

namespace PointOfSaleWebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly PointOfSale.POSDbContext context;

        public ProductController(POSDbContext context)
        {
            this.context = context;
        }

        
        // in web api / web projects actions are asynchronous
        // web api endpoints returns response not views
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var data = await context.Products.ToListAsync();
            return Ok(data); // 200 status code
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var data = await context.Products.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return data; // 200 status code
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Update(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(product);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(int id)
        {
            var std = await context.Products.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            context.Products.Remove(std);
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
