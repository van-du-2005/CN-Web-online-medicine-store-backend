using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;

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
            return Ok(await _context.DanhMucs.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> ThemDanhMuc([FromBody] DanhMuc model)
        {
            model.MaDanhMuc = Guid.NewGuid();
            _context.DanhMucs.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Thêm danh mục thành công!", data = model });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> SuaDanhMuc(Guid id, [FromBody] DanhMuc model)
        {
            if (id != model.MaDanhMuc) return BadRequest(new { message = "ID không khớp!" });
            
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thành công!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> XoaDanhMuc(Guid id)
        {
            var dm = await _context.DanhMucs.FindAsync(id);
            if (dm == null) return NotFound(new { message = "Không tìm thấy danh mục!" });

            _context.DanhMucs.Remove(dm);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xóa thành công!" });
        }
    }
}