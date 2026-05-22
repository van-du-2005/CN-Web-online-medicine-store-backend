using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMedicineStoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NhaCungCapsController : ControllerBase
    {
        private readonly OnlineMedicineStoreCNWDbContext _context;

        public NhaCungCapsController(OnlineMedicineStoreCNWDbContext context)
        {
            _context = context;
        }

        // 1. LẤY DANH SÁCH (Hỗ trợ Frontend tự phân trang)
        [HttpGet]
        public async Task<IActionResult> GetNhaCungCaps()
        {
            // Lấy tất cả, sắp xếp mới nhất lên đầu để Angular tự lo việc phân trang và tìm kiếm
            var list = await _context.NhaCungCaps.OrderByDescending(n => n.TenCongTy).ToListAsync();
            return Ok(list);
        }

        // 2. THÊM MỚI
        [HttpPost]
        public async Task<IActionResult> PostNhaCungCap([FromBody] NhaCungCap model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.MaNhaCungCap = Guid.NewGuid();
            _context.NhaCungCaps.Add(model);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Thêm thành công!" });
        }

        // 3. CẬP NHẬT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhaCungCap(Guid id, [FromBody] NhaCungCap model)
        {
            if (id != model.MaNhaCungCap) return BadRequest(new { message = "ID không khớp!" });

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Cập nhật thành công!" });
        }

        // 4. XÓA (Có bắt lỗi khóa ngoại)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhaCungCap(Guid id)
        {
            var item = await _context.NhaCungCaps.FindAsync(id);
            if (item == null) return NotFound();
            
            try 
            {
                _context.NhaCungCaps.Remove(item);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Xóa thành công!" });
            } 
            catch (Exception) 
            {
                return BadRequest(new { message = "Lỗi: Đang có phiếu nhập sử dụng nhà cung cấp này nên không thể xóa!" });
            }
        }
    }
}