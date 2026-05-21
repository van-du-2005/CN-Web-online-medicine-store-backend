using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhieuNhapsController : ControllerBase
    {
        private readonly OnlineMedicineStoreCNWDbContext _context;

        public PhieuNhapsController(OnlineMedicineStoreCNWDbContext context)
        {
            _context = context;
        }

        // 1. LẤY DANH SÁCH PHIẾU NHẬP (Kèm theo thông tin Nhà cung cấp)
        [HttpGet]
        public async Task<IActionResult> GetPhieuNhaps()
        {
            var data = await _context.PhieuNhaps
                // Nếu sếp có Include tới bảng NhaCungCap thì mở dòng dưới ra xài nha
                // .Include(p => p.NhaCungCap) 
                .OrderByDescending(p => p.NgayNhap)
                .ToListAsync();
            return Ok(data);
        }

        // 2. DUYỆT PHIẾU NHẬP
        [HttpPut("duyet/{id}")]
        public async Task<IActionResult> DuyetPhieuNhap(Guid id)
        {
            var phieu = await _context.PhieuNhaps.FindAsync(id);
            if (phieu == null) return NotFound(new { message = "Không tìm thấy phiếu nhập!" });

            // Đổi trạng thái phiếu nhập (sếp tự căn chỉnh theo field Trạng thái trong DB nhé)
            phieu.TrangThai = "Đã duyệt"; // Ví dụ true là Đã duyệt, false là Chờ duyệt

            _context.Entry(phieu).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Đã duyệt phiếu nhập kho thành công!" });
        }
    }
}