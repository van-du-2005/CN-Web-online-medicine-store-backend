using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.Models;
using OnlineMedicineStoreBackend.Services;

namespace OnlineMedicineStoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly IUserService _userService;

        public NguoiDungController(IUserService userService)
        {
            _userService = userService;
        }

        // Lấy danh sách tất cả người dùng (Admin)
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Xem chi tiết 1 người dùng
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("Không tìm thấy người dùng.");
            return Ok(user);
        }

        // Xóa người dùng (Admin)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var thanhCong = await _userService.DeleteUserAsync(id);
            if (!thanhCong) return NotFound("Không tìm thấy người dùng để xóa.");
            return Ok("Xóa người dùng thành công!");
        }
    }
}