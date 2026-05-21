using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;

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

        [HttpGet]
        public async Task<IActionResult> GetNhaCungCaps()
        {
            return Ok(await _context.NhaCungCaps.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> ThemNhaCungCap([FromBody] NhaCungCap model)
        {
            model.MaNhaCungCap = Guid.NewGuid();
            _context.NhaCungCaps.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Thêm nhà cung cấp thành công!", data = model });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> SuaNhaCungCap(Guid id, [FromBody] NhaCungCap model)
        {
            if (id != model.MaNhaCungCap) return BadRequest(new { message = "ID không khớp!" });
            
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thành công!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> XoaNhaCungCap(Guid id)
        {
            var ncc = await _context.NhaCungCaps.FindAsync(id);
            if (ncc == null) return NotFound();

            _context.NhaCungCaps.Remove(ncc);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xóa thành công!" });
        }
    }
}