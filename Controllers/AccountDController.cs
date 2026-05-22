using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.DTOs.account;
using OnlineMedicineStoreBackend.DTOs.auth;
using OnlineMedicineStoreBackend.Services;
using System.Security.Claims;

namespace OnlineMedicineStoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] //Chỉ user đã đăng nhập (có Token) mới được gọi API này
    public class AccountDController : ControllerBase
    {
        private readonly IAccountDService _accountService;

        public AccountDController(IAccountDService accountService)
        {
            _accountService = accountService;
        }

        // Lấy User ID từ JWT Token hiện tại
        private Guid GetCurrentUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _accountService.GetProfileAsync(GetCurrentUserId());
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto dto)
        {
            var result = await _accountService.UpdateProfileAsync(GetCurrentUserId(), dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<string> { Success = false, Message = "Dữ liệu không hợp lệ." });
            var result = await _accountService.ChangePasswordAsync(GetCurrentUserId(), dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders([FromQuery] string filter = "TatCa")
        {
            var result = await _accountService.GetOrdersAsync(GetCurrentUserId(), filter);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("orders/{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var result = await _accountService.CancelOrderAsync(GetCurrentUserId(), orderId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("addresses")]
        public async Task<IActionResult> GetAddresses()
        {
            var result = await _accountService.GetAddressesAsync(GetCurrentUserId());
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("addresses")]
        public async Task<IActionResult> AddAddress([FromBody] AddressDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<string> { Success = false, Message = "Dữ liệu không hợp lệ." });
            var result = await _accountService.AddAddressAsync(GetCurrentUserId(), dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("addresses/{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            var result = await _accountService.DeleteAddressAsync(GetCurrentUserId(), addressId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("addresses/{addressId}/default")]
        public async Task<IActionResult> SetDefaultAddress(Guid addressId)
        {
            var result = await _accountService.SetDefaultAddressAsync(GetCurrentUserId(), addressId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}