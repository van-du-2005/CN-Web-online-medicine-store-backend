using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.Areas.Admin.ViewModels;
using OnlineMedicineStoreBackend.Services.Admin;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMedicineStoreBackend.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) => _userService = userService;

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new { Title = "Quản lý Người dùng" });
        }

        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers([FromQuery] string role = "TatCa", [FromQuery] string status = "TatCa", [FromQuery] int page = 1)
        {
            var data = await _userService.GetUsersAsync(role, status, page, 5); // Limit mặc định là 5 theo yêu cầu
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var data = await _userService.GetUserForEditAsync(id);
            if (data == null) return NotFound(new { success = false, message = "Không tìm thấy người dùng." });
            return Ok(data);
        }

        [HttpPost("save-user")]
        public async Task<IActionResult> SaveUser([FromBody] UserUpsertViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { success = false, message = string.Join("\n", errors) });
            }

            var (success, error) = await _userService.UpsertUserAsync(model);
            if (!success)
            {
                return BadRequest(new { success, message = error });
            }
            return Ok(new { success, message = error });
        }

        [HttpPost("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var success = await _userService.DeleteUserAsync(id);
            return Ok(new { success });
        }
    }
}