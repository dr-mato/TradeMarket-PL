using Business.Interfaces;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptsController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _receiptService.GetAllAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            await _receiptService.GetByIdAsync(id);
            return Ok();
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetDetails(int id)
        {
            await _receiptService.GetReceiptDetailsAsync(id);
            return Ok();
        }

        [HttpGet("{id}/sum")]
        public async Task<IActionResult> GetSum(int id)
        {
            await _receiptService.ToPayAsync(id);
            return Ok();
        }

        [HttpGet("period")]
        public async Task<IActionResult> GetByPeriod(DateTime startDate, DateTime endDate)
        {
            await _receiptService.GetReceiptsByPeriodAsync(startDate, endDate);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReceiptModel receipt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _receiptService.AddAsync(receipt);

                if (receipt == null)
                {
                    return BadRequest();
                }

                return CreatedAtAction(nameof(Get), new { id = receipt.Id }, receipt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int Id, [FromBody] ReceiptModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var receipt = await _receiptService.GetByIdAsync(Id);

                if (receipt == null)
                {
                    return NotFound();
                }

                receipt = value;

                if (value == null)
                {
                    return BadRequest();
                }

                await _receiptService.UpdateAsync(receipt);

                return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/products/add/{productId}/{quantity}")]
        public async Task<IActionResult> AddProduct(int Id, int productId, int quantity)
        {
            await _receiptService.AddProductAsync(Id, productId, quantity);
            return Ok();
        }

        [HttpPut("{id}/products/remove/{productId}/{quantity}")]
        public async Task<IActionResult> RemoveProduct(int Id, int productId, int quantity)
        {
            await _receiptService.RemoveProductAsync(Id, productId, quantity);
            return Ok();
        }

        [HttpPut("{id}/checkout")]
        public async Task<IActionResult> Checkout(int id)
        {
            await _receiptService.CheckOutAsync(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceipt(int id)
        {
            await _receiptService.DeleteAsync(id);
            return Ok();
        }   
    }
}
