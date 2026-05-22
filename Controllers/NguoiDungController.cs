using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.DTOs;
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

        // 1. Lấy danh sách
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // 2. Lấy chi tiết
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { message = "Không tìm thấy người dùng." });
            return Ok(user);
        }

        // 3. Thêm mới
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] TaoNguoiDungDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newUser = await _userService.AddUserAsync(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        // 4. Cập nhật thông tin
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] CapNhatNguoiDungDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedUser = await _userService.UpdateUserAsync(id, dto);
            if (updatedUser == null) return NotFound(new { message = "Không tìm thấy người dùng." });
            return Ok(updatedUser);
        }

        // 5. Khóa / Mở khóa
        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var result = await _userService.ToggleStatusAsync(id);
            if (!result) return NotFound(new { message = "Không tìm thấy người dùng." });
            return Ok(new { message = "Đã thay đổi trạng thái thành công!" });
        }

        // 6. Xóa
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var thanhCong = await _userService.DeleteUserAsync(id);
            if (!thanhCong) return NotFound(new { message = "Không tìm thấy người dùng để xóa." });
            return Ok(new { message = "Xóa người dùng thành công!" });
        }
    }
}