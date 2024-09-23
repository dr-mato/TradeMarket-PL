using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpGet("popular-products")]
        public async Task<IActionResult> GetPopularProducts(int productCount)
        {
            await _statisticService.GetMostPopularProductsAsync(productCount);
            return Ok();
        }

        [HttpGet("customer/{id}/{productCount}")]
        public async Task<IActionResult> GetCustomerFavouriteProducts(int productCount, int customerId)
        {
            await _statisticService.GetCustomersMostPopularProductsAsync(productCount, customerId);
            return Ok();
        }

        [HttpGet("activity/{customerCount}/period")]
        public async Task<IActionResult> GetActiveCustomers(int customerCount, DateTime startDate, DateTime endDate)
        {
            await _statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate);
            return Ok();
        }

        [HttpGet("income/{categoryId}/period")]
        public async Task<IActionResult> GetCategoryIncome(int categoryId, DateTime startDate, DateTime endDate)
        {
            await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
            return Ok();
        }

    }
}
