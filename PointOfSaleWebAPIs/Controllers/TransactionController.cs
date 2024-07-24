using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly POSDbContext context;
        private readonly ILogger<TransactionController> _logger;


        public TransactionController(POSDbContext context, ILogger<TransactionController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [HttpPost("AddProductToSale/{id}/{quantity}")]
        public IActionResult AddProductToSale(int id, int quantity)
        {
            try
            {
                bool added = EFTransaction.AddProductToSaleApi(context, id, quantity);
                if (added)
                {
                    _logger.LogInformation("Product added To sale");
                    return Ok("Product added to sale");
                }
                else
                {
                    _logger.LogWarning("Product is not added To sale");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
           
        }
        [HttpGet("ViewProductsinSale")]
        public IActionResult ViewSaleProducts()
        {
            try
            {
                var saleProducts = context.SaleProducts.ToList();
                _logger.LogInformation($"Products Count in Sale : {saleProducts.Count}");
                return Ok(saleProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
           
        }
        [HttpPut("UpdateProductsInSale/{id}/{quantity}")]
        public IActionResult UpdateProductsInSale(int id, int quantity)
        {
            try
            {
                bool updated = EFTransaction.UpdateProductsInSaleApi(context, id, quantity);
                if (updated)
                {
                    _logger.LogInformation("Product updated in sale");
                    return Ok("Products updated in sale");
                }
                else
                {
                    _logger.LogWarning("Product not added To sale");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
          
            
        }

        [HttpGet("GenerateReceipt")]
        public IActionResult GenerateReceipt()
        {
            try
            {
                var receipt = EFTransaction.GenerateReceiptAPI(context);
                _logger.LogInformation($"{receipt}");   
                return Ok(receipt);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
            
        }

        

        [HttpGet("CalculateTotalAmount")]
        public IActionResult CalculateTotalAmount()
        {
            try
            {
                double totalAmount = EFTransaction.CalculateTotalAmount(context);
                _logger.LogInformation($"Total amount: {totalAmount}");
                return Ok(totalAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
            
        }
    }
}
