using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data; 
using OnlineMedicineStoreBackend.Models; 

namespace OnlineMedicineStoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThuocsController : ControllerBase
    {
        // Đã sửa tên DbContext cho khớp với project của sếp
        private readonly OnlineMedicineStoreCNWDbContext _context;

        public ThuocsController(OnlineMedicineStoreCNWDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // API 1: LẤY DANH SÁCH THUỐC
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> GetDanhSachThuoc(string? search)
        {
            var query = _context.Thuocs.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(t => t.TenThuoc.ToLower().Contains(searchLower));
            }

            var result = await query.ToListAsync();
            return Ok(result); 
        }

        // ==========================================
        // API 2: THÊM THUỐC MỚI
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> ThemThuoc([FromBody] Thuoc model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            model.MaThuoc = Guid.NewGuid();

            _context.Thuocs.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm thuốc thành công!", data = model });
        }
        // ==========================================
        // API 3: LẤY CHI TIẾT 1 THUỐC THEO ID (Dùng để Angular hiển thị lên form Sửa)
        // ==========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetThuoc(Guid id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return NotFound(new { message = "Không tìm thấy thuốc!" });
            return Ok(thuoc);
        }

        // ==========================================
        // API 4: SỬA THUỐC (Thay cho hàm POST Edit cũ)
        // ==========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> SuaThuoc(Guid id, [FromBody] Thuoc model)
        {
            // Kiểm tra xem ID trên link API và ID trong cục data gửi lên có khớp không
            if (id != model.MaThuoc) return BadRequest(new { message = "ID thuốc không hợp lệ!" });
            
            _context.Entry(model).State = EntityState.Modified;

            try 
            { 
                await _context.SaveChangesAsync(); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Thuocs.Any(e => e.MaThuoc == id)) return NotFound(new { message = "Thuốc không tồn tại!" });
                else throw;
            }

            return Ok(new { message = "Cập nhật thông tin thuốc thành công!" });
        }

        // ==========================================
        // API 5: XÓA THUỐC
        // ==========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> XoaThuoc(Guid id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return NotFound(new { message = "Không tìm thấy thuốc để xóa!" });

            _context.Thuocs.Remove(thuoc);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thuốc thành công!" });
        }
    }
}