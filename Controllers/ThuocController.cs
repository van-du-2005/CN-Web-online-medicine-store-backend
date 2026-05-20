using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnlineMedicineStoreBackend.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ThuocController : ControllerBase
    {
        private readonly OnlineMedicineStoreCNWDbContext _context;

        public ThuocController(OnlineMedicineStoreCNWDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. DANH SÁCH & TÌM KIẾM, LỌC
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string? maDanhMuc, [FromQuery] string? trangThai, [FromQuery] string? search)
        {
            // 1. TỐI ƯU HÓA: Đếm số lượng trực tiếp dưới Database (Chống tràn RAM)
            var total = await _context.Thuocs.CountAsync();
            var sapHetHang = await _context.Thuocs.CountAsync(t => t.TonKhoHienTai > 0 && t.TonKhoHienTai <= 10);
            var hetHang = await _context.Thuocs.CountAsync(t => t.TonKhoHienTai <= 0);

            // 2. KHỞI TẠO TRUY VẤN
            var query = _context.Thuocs.Include(t => t.DanhMuc).AsQueryable();

            // 3. LỌC THEO TÌM KIẾM
            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(t => t.TenThuoc.ToLower().Contains(searchLower) || 
                                        (t.TenKhoaHoc != null && t.TenKhoaHoc.ToLower().Contains(searchLower)));
            }

            // 4. LỌC THEO DANH MỤC (Xử lý an toàn chống sập khi nhận chuỗi rỗng)
            if (!string.IsNullOrEmpty(maDanhMuc) && Guid.TryParse(maDanhMuc, out Guid danhMucId))
            {
                query = query.Where(t => t.MaDanhMuc == danhMucId);
            }

            // 5. LỌC THEO TRẠNG THÁI
            if (!string.IsNullOrEmpty(trangThai))
            {
                if (trangThai == "conhang") query = query.Where(t => t.TonKhoHienTai > 10);
                else if (trangThai == "saphethang") query = query.Where(t => t.TonKhoHienTai > 0 && t.TonKhoHienTai <= 10);
                else if (trangThai == "hethang") query = query.Where(t => t.TonKhoHienTai <= 0);
            }

            var danhMucs = await _context.DanhMucs
                .Where(d => d.TrangThai)
                .Select(d => new { d.MaDanhMuc, d.TenDanhMuc })
                .ToListAsync();

            var result = await query.OrderBy(t => t.TenThuoc).ToListAsync();

            return Ok(new
            {
                total,
                sapHetHang,
                hetHang,
                danhMucs,
                currentSearch = search,
                currentTrangThai = trangThai,
                data = result
            });
        }

        // ==========================================
        // 2. MÀN HÌNH THÊM MỚI (GET)
        // ==========================================
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var danhMucs = await _context.DanhMucs
                .Where(d => d.TrangThai)
                .Select(d => new { d.MaDanhMuc, d.TenDanhMuc })
                .ToListAsync();
            return Ok(new { danhMucs });
        }

        // ==========================================
        // 3. XỬ LÝ LƯU THÊM MỚI (POST)
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Thuoc model, IFormFile? imageFile)
        {
            ModelState.Remove("DanhMuc");
            ModelState.Remove("ChiTietDonHangs");
            ModelState.Remove("ChiTietGioHangs");
            ModelState.Remove("LoThuocs");

            if (ModelState.IsValid)
            {
                model.MaThuoc = Guid.NewGuid();

                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
                    var directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                    using (var stream = new FileStream(filePath, FileMode.Create)) { await imageFile.CopyToAsync(stream); }
                    model.HinhAnh = "/uploads/" + fileName; 
                }

                _context.Thuocs.Add(model);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Thêm mới thuốc thành công.", data = model });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            var danhMucs = await _context.DanhMucs
                .Where(d => d.TrangThai)
                .Select(d => new { d.MaDanhMuc, d.TenDanhMuc })
                .ToListAsync();
            return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ.", errors, data = model, danhMucs });
        }

        // ==========================================
        // 4. MÀN HÌNH CHỈNH SỬA (GET)
        // ==========================================
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest(new { success = false, message = "Lỗi: Không tìm thấy ID thuốc." });

            var thuoc = await _context.Thuocs.FirstOrDefaultAsync(t => t.MaThuoc == id);
            if (thuoc == null) return NotFound(new { success = false, message = $"Lỗi: Không tìm thấy dữ liệu cho mã thuốc {id}" });

            var danhMucs = await _context.DanhMucs
                .Where(d => d.TrangThai)
                .Select(d => new { d.MaDanhMuc, d.TenDanhMuc })
                .ToListAsync();

            return Ok(new { success = true, data = thuoc, danhMucs });
        }

        // ==========================================
        // 5. XỬ LÝ LƯU CHỈNH SỬA (POST)
        // ==========================================
        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromForm] Thuoc model, IFormFile? imageFile)
        {
            model.MaThuoc = id; // Ép khớp ID

            ModelState.Remove("DanhMuc");
            ModelState.Remove("ChiTietDonHangs");
            ModelState.Remove("ChiTietGioHangs");
            ModelState.Remove("LoThuocs");

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
                        var directory = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                        using (var stream = new FileStream(filePath, FileMode.Create)) { await imageFile.CopyToAsync(stream); }
                        model.HinhAnh = "/uploads/" + fileName; 
                    }
                    else
                    {
                        var existingThuoc = await _context.Thuocs.AsNoTracking().FirstOrDefaultAsync(t => t.MaThuoc == id);
                        if (existingThuoc != null) model.HinhAnh = existingThuoc.HinhAnh;
                    }

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Thuocs.Any(e => e.MaThuoc == id)) 
                        return NotFound(new { success = false, message = "Lỗi: Dữ liệu đã bị xóa bởi người khác." });
                    else 
                        throw;
                }
                return Ok(new { success = true, message = "Cập nhật thuốc thành công.", data = model });
            }
            
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            var danhMucs = await _context.DanhMucs
                .Where(d => d.TrangThai)
                .Select(d => new { d.MaDanhMuc, d.TenDanhMuc })
                .ToListAsync();
            return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ.", errors, data = model, danhMucs });
        }

        // ==========================================
        // 6. XỬ LÝ XÓA (DELETE)
        // ==========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy thuốc cần xóa." });
            }
            _context.Thuocs.Remove(thuoc);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Xóa thuốc thành công." });
        }
    }
}