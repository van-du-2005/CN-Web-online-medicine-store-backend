using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.DTOs;
using OnlineMedicineStoreBackend.Services;
using System.Security.Claims;

namespace OnlineMedicineStoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Đặt Authorize ở mức class để bảo vệ toàn bộ API, Angular bắt buộc phải gửi Token
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        // Đã loại bỏ IAuthService vì .NET cung cấp sẵn cách lấy Claim chuẩn và an toàn hơn
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private Guid GetUserId()
        {
            // Tối ưu hóa việc lấy UserID trực tiếp từ Bearer Token
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdClaim, out Guid userId) ? userId : Guid.Empty;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            if (userId == Guid.Empty) 
                return Unauthorized(new { success = false, message = "Token không hợp lệ." });

            var cartDto = await _cartService.GetCartAsync(userId);
            return Ok(new { success = true, data = cartDto ?? new CartDto() });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            if (request == null) 
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });

            var userId = GetUserId();
            if (userId == Guid.Empty) 
                return Unauthorized(new { success = false, message = "Token không hợp lệ." });

            bool isSuccess = await _cartService.AddCartAsync(userId, request.ProductId, request.Quantity);
            
            if (isSuccess) 
                return Ok(new { success = true, message = "Thêm vào giỏ hàng thành công." });
                
            return BadRequest(new { success = false, message = "Thêm thất bại." });
        }

        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            if (request.Quantity <= 0) 
                return BadRequest(new { success = false, message = "Số lượng phải lớn hơn 0." });

            var userId = GetUserId();
            if (userId == Guid.Empty) 
                return Unauthorized(new { success = false, message = "Token không hợp lệ." });

            var result = await _cartService.UpdateCartItemQuantityAsync(userId, request.ProductId, request.Quantity);
            
            if (result) 
                return Ok(new { success = true, message = "Cập nhật thành công." });
                
            return BadRequest(new { success = false, message = "Cập nhật thất bại." });
        }

        // Tối ưu thành HttpDelete chuẩn RESTful, truyền tham số trực tiếp trên URL thay vì Body
        [HttpDelete("remove-item/{productId}")]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {
            var userId = GetUserId();
            if (userId == Guid.Empty) 
                return Unauthorized(new { success = false, message = "Token không hợp lệ." });

            var result = await _cartService.RemoveCartItemAsync(userId, productId);
            
            if (result) 
                return Ok(new { success = true, message = "Xóa thành công." });
                
            return BadRequest(new { success = false, message = "Xóa thất bại." });
        }
    }
}