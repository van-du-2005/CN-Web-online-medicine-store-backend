using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.Services.admin;
using System.Security.Claims;

namespace OnlineMedicineStoreBackend.Controllers.admin
{
    public class OrderController : AdminBaseController
    {
        private readonly IOrderServiceAdmin _orderService;

        public OrderController(IOrderServiceAdmin orderService)
        {
            _orderService = orderService;
        }

        
        private Guid GetCurrentUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
        }

        [HttpGet("counts")]
        public async Task<IActionResult> GetCounts()
        {
            var result = await _orderService.GetFilterCountsAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] string status = "TatCa", [FromQuery] int page = 1, [FromQuery] int limit = 15)
        {
            var result = await _orderService.GetOrdersAsync(status, page, limit);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var result = await _orderService.GetOrderDetailAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status)) 
                return BadRequest(new { Success = false, Message = "Trạng thái không hợp lệ." });

            var currentUserId = GetCurrentUserId();
            var result = await _orderService.UpdateOrderStatusAsync(id, status, currentUserId);
            
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}