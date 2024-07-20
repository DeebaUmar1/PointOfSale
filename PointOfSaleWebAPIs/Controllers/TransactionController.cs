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
        public IActionResult AddProductToSale([FromBody] SaleProducts saleProduct)
        {
            EFTransaction.Add(context, saleProduct);
            return Ok("Product added to sale");
        }

        [HttpPut("UpdateProductsInSale")]
        public IActionResult UpdateProductsInSale()
        {
            EFTransaction.UpdateProductsInSale(context);
            return Ok("Products updated in sale");
        }

        [HttpGet("GenerateReceipt")]
        public IActionResult GenerateReceipt()
        {
            EFTransaction.Generate(context);
            return Ok("Receipt generated");
        }

        [HttpGet("PrintTotalAmount")]
        public IActionResult PrintTotalAmount()
        {
            EFTransaction.PrintTotalAmount(context);
            return Ok();
        }

        [HttpGet("CalculateTotalAmount")]
        public IActionResult CalculateTotalAmount()
        {
            double totalAmount = EFTransaction.CalculateTotalAmount(context);
            return Ok(totalAmount);
        }
    }
}
