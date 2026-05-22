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
    public class PhieuNhapsController : ControllerBase
    {
        private readonly OnlineMedicineStoreCNWDbContext _context;

        public PhieuNhapsController(OnlineMedicineStoreCNWDbContext context)
        {
            _context = context;
        }

        // 1. LẤY DANH SÁCH PHIẾU NHẬP
        [HttpGet]
        public async Task<IActionResult> GetPhieuNhaps()
        {
            // Kết nối sang bảng Nhà Cung Cấp để lấy tên công ty
            var query = _context.PhieuNhaps.Include(p => p.NhaCungCap)
                                           .OrderByDescending(p => p.NgayNhap);
            
            var list = await query.ToListAsync();
            
            // Ngắt vòng lặp JSON 
            foreach (var item in list)
            {
                if (item.NhaCungCap != null) item.NhaCungCap.PhieuNhaps = null; 
            }
            
            return Ok(list);
        }

        // 2. LẤY CHI TIẾT (Angular sẽ tự xử lý trên Frontend nên API này dự phòng)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChiTiet(Guid id) // Nếu DB sếp xài int thì đổi Guid thành int nhé
        {
            var phieu = await _context.PhieuNhaps
                .Include(p => p.NhaCungCap)
                .FirstOrDefaultAsync(p => p.MaPhieuNhap == id);
                
            if (phieu == null) return NotFound();
            if (phieu.NhaCungCap != null) phieu.NhaCungCap.PhieuNhaps = null;
            return Ok(phieu);
        }

        // 3. CẬP NHẬT TRẠNG THÁI (Phê duyệt / Từ chối)
        [HttpPost("{id}/trangthai")]
        public async Task<IActionResult> CapNhatTrangThai(Guid id, [FromQuery] string trangThai) // Nếu DB sếp xài int thì đổi Guid thành int nhé
        {
            var phieu = await _context.PhieuNhaps.FindAsync(id);
            if (phieu == null) return NotFound();

            phieu.TrangThai = trangThai; // "DaDuyet" hoặc "TuChoi"
            
            // Tương lai sếp sẽ viết logic cộng tồn kho vào đây (khi DaDuyet)
            
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật trạng thái thành công!" });
        }
        
    }

    
}