using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.Services.admin;

namespace OnlineMedicineStoreBackend.Controllers.admin
{
    public class DashboardController : AdminBaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary([FromQuery] string filter = "thangnay")
        {
            var result = await _dashboardService.GetSummaryAsync(filter);
            return Ok(result);
        }

        [HttpGet("chart")]
        public async Task<IActionResult> GetChart([FromQuery] string filter = "thangnay")
        {
            var result = await _dashboardService.GetChartAsync(filter);
            return Ok(result);
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopProducts([FromQuery] string filter = "thangnay", [FromQuery] int page = 1, [FromQuery] int limit = 5)
        {
            var result = await _dashboardService.GetTopProductsAsync(filter, page, limit);
            return Ok(result);
        }

        [HttpGet("top-customers")]
        public async Task<IActionResult> GetTopCustomers([FromQuery] string filter = "thangnay", [FromQuery] int page = 1, [FromQuery] int limit = 5)
        {
            var result = await _dashboardService.GetTopCustomersAsync(filter, page, limit);
            return Ok(result);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock([FromQuery] int page = 1, [FromQuery] int limit = 15)
        {
            var result = await _dashboardService.GetLowStockProductsAsync(page, limit);
            return Ok(result);
        }
    }
}