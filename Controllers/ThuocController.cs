using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.Models;
using OnlineMedicineStoreBackend.Services;

namespace OnlineMedicineStoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThuocController : ControllerBase
    {
        private readonly IProductService _productService;

        public ThuocController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDanhSachThuoc([FromQuery] string? tuKhoa, [FromQuery] Guid? maDanhMuc)
        {
            var danhSach = await _productService.GetDanhSachThuocAsync(tuKhoa, maDanhMuc);
            return Ok(danhSach);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChiTietThuoc(Guid id)
        {
            var thuoc = await _productService.GetChiTietThuocAsync(id);
            if (thuoc == null) return NotFound("Không tìm thấy sản phẩm này.");
            return Ok(thuoc);
        }

        // --- API THÊM SẢN PHẨM MỚI (Dành cho Admin) ---
        // Sử dụng phương thức POST
        [HttpPost]
        public async Task<IActionResult> ThemThuoc([FromBody] Thuoc thuocMoi)
        {
            var ketQua = await _productService.AddThuocAsync(thuocMoi);
            return Ok(ketQua);
        }

        // --- API XÓA SẢN PHẨM (Dành cho Admin) ---
        // Sử dụng phương thức DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> XoaThuoc(Guid id)
        {
            var thanhCong = await _productService.DeleteThuocAsync(id);
            if (!thanhCong) return NotFound("Không tìm thấy sản phẩm để xóa.");
            return Ok("Xóa thành công!");
        }
    }
}