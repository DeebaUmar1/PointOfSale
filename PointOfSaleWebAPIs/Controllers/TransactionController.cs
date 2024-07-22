using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSale;
using PointOfSale.Data;
using PointOfSale.Services;
using System;
using System.Linq;

namespace PointOfSaleWebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly POSDbContext context;

        public TransactionController(POSDbContext context)
        {
            this.context = context;
        }

        [HttpPost("AddProductToSale")]
        public IActionResult AddProductToSale(int id, int quantity)
        {
            bool added = EFTransaction.AddProductToSaleApi(context, id, quantity);
            if (added)
            {

                return Ok("Product added to sale");
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("ViewProductsinSale")]
        public IActionResult ViewSaleProducts()
        {
            var saleProducts = context.SaleProducts.ToList();
            return Ok(saleProducts);
        }
        [HttpPut("UpdateProductsInSale")]
        public IActionResult UpdateProductsInSale(int id, int quantity)
        {
            bool updated = EFTransaction.UpdateProductsInSaleApi(context, id, quantity);
            if (updated)
            {

                return Ok("Products updated in sale");
            }
            else
            {
                return BadRequest();
            }
            
        }

        [HttpGet("GenerateReceipt")]
        public IActionResult GenerateReceipt()
        {
            var receipt = EFTransaction.GenerateReceiptAPI(context);
            return Ok(receipt);
        }

        

        [HttpGet("CalculateTotalAmount")]
        public IActionResult CalculateTotalAmount()
        {
            double totalAmount = EFTransaction.CalculateTotalAmount(context);
            return Ok(totalAmount);
        }
    }
}
