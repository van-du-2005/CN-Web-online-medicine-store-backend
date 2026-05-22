using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMedicineStoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThuocsController : ControllerBase
    {
        private readonly OnlineMedicineStoreCNWDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ThuocsController(OnlineMedicineStoreCNWDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ==========================================
        // API 1: LẤY DANH SÁCH THUỐC (Đã bọc thép chống lỗi vòng lặp)
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> GetDanhSachThuoc(string? search)
        {
            // 1. Dùng Include để C# qua nhà Danh Mục lấy tên
            var query = _context.Thuocs.Include(t => t.DanhMuc).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(t => t.TenThuoc.ToLower().Contains(searchLower));
            }

            var result = await query.ToListAsync();

            // 2. NGẮT VÒNG LẶP JSON: Đảm bảo server không bị quá tải bộ nhớ và crash
            foreach (var item in result)
            {
                if (item.DanhMuc != null)
                {
                    // Chặn Danh Mục gọi ngược lại danh sách Thuốc
                    item.DanhMuc.Thuocs = null; 
                }
            }

            return Ok(result);
        }

        // ==========================================
        // API 2: THÊM THUỐC MỚI (Bản Bọc Thép)
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> ThemThuoc([FromForm] Thuoc model, IFormFile? imageFile)
        {
            ModelState.Remove("DanhMuc");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                model.MaThuoc = Guid.NewGuid();

                if (imageFile != null && imageFile.Length > 0)
                {
                    var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var uploadsFolder = Path.Combine(webRootPath, "images", "products");

                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    model.HinhAnh = "/images/products/" + uniqueFileName;
                }

                _context.Thuocs.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Thêm thuốc thành công rực rỡ!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi C#: " + ex.Message });
            }
        }

        // ==========================================
        // API 3: LẤY CHI TIẾT 1 THUỐC THEO ID
        // ==========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetThuoc(Guid id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return NotFound(new { message = "Không tìm thấy thuốc!" });
            return Ok(thuoc);
        }

        // ==========================================
        // API 4: SỬA THUỐC (Bản Nâng Cấp Nhận Ảnh)
        // ==========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> SuaThuoc(Guid id, [FromForm] Thuoc model, IFormFile? imageFile)
        {
            if (id != model.MaThuoc) return BadRequest(new { message = "ID thuốc không hợp lệ!" });

            ModelState.Remove("DanhMuc"); 
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var thuocCu = await _context.Thuocs.AsNoTracking().FirstOrDefaultAsync(t => t.MaThuoc == id);
                if (thuocCu == null) return NotFound(new { message = "Thuốc không tồn tại!" });

                if (imageFile != null && imageFile.Length > 0)
                {
                    var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var uploadsFolder = Path.Combine(webRootPath, "images", "products");

                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    model.HinhAnh = "/images/products/" + uniqueFileName;
                }
                else
                {
                    model.HinhAnh = thuocCu.HinhAnh;
                }

                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật thông tin thuốc thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi C# Sửa Thuốc: " + ex.Message });
            }
        }

        // ==========================================
        // API 5: XÓA THUỐC
        // ==========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> XoaThuoc(Guid id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return NotFound();

            try
            {
                _context.Thuocs.Remove(thuoc);
                await _context.SaveChangesAsync();
                return Ok(); 
            }
            catch (Exception) 
            {
                return BadRequest(new { message = "Không thể xóa! Thuốc này đang bị kẹt ràng buộc." });
            }
        }
    }
}