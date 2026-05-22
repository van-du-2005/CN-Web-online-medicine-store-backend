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
    public class DanhMucsController : ControllerBase
    {
        private readonly OnlineMedicineStoreCNWDbContext _context;

        public DanhMucsController(OnlineMedicineStoreCNWDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDanhMucs()
        {
            // Lấy danh sách, tự động trả về cả trường TrangThai
            var danhMucs = await _context.DanhMucs.OrderByDescending(d => d.MaDanhMuc).ToListAsync();
            return Ok(danhMucs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDanhMuc(Guid id)
        {
            var dm = await _context.DanhMucs.FindAsync(id);
            if (dm == null) return NotFound();
            return Ok(dm);
        }

        [HttpPost]
        public async Task<IActionResult> PostDanhMuc([FromBody] DanhMuc model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.MaDanhMuc = Guid.NewGuid();
            _context.DanhMucs.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Thêm danh mục thành công!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDanhMuc(Guid id, [FromBody] DanhMuc model)
        {
            if (id != model.MaDanhMuc) return BadRequest(new { message = "ID không khớp!" });

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thành công!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDanhMuc(Guid id)
        {
            var dm = await _context.DanhMucs.FindAsync(id);
            if (dm == null) return NotFound();
            
            try 
            {
                _context.DanhMucs.Remove(dm);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Xóa thành công!" });
            } 
            catch (Exception) 
            {
                return BadRequest(new { message = "Không thể xóa! Danh mục này đang có thuốc bên trong." });
            }
        }
    }
}