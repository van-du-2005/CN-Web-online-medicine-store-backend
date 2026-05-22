using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.DTOs.auth;
using OnlineMedicineStoreBackend.Services.auth;

namespace OnlineMedicineStoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Token))
            {
                return BadRequest(new ApiResponse<string> { Success = false, Message = "Dữ liệu gửi lên không hợp lệ." });
            }

            var result = await _authService.LoginWithGoogleAsync(dto.Token);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<string> { Success = false, Message = "Dữ liệu không hợp lệ." });
            var result = await _authService.RegisterAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var result = await _authService.VerifyOtpAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(new ApiResponse<string> { Success = false, Message = "Dữ liệu không hợp lệ." });
            
            var result = await _authService.LoginAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}