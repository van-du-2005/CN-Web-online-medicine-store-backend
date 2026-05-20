
using OnlineMedicineStore.ViewModels;
using OnlineMedicineStore.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMedicineStore.Data;
using OnlineMedicineStore.Models.Entities;
using OnlineMedicineStore.ViewModels;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Linq;
using System.Security.Claims;
using OnlineMedicineStore.Data;
using System.Threading.Tasks;

namespace OnlineMedicineStore.Controllers
{
    // KHÔNG dùng [Authorize] vì scheme mặc định là JwtBearer (đọc header),
    // còn app dùng JwtCookieMiddleware để set User từ cookie.
    // Thay vào đó mỗi action tự kiểm tra User.Identity.IsAuthenticated.
    public class AccountController : Controller
    {
        private readonly OnlineMedicineStoreDbContext _context;

        public AccountController(OnlineMedicineStoreDbContext context)
        {
            _context = context;
        }

        // ── Helper: kiểm tra đăng nhập, redirect nếu chưa ───────
        private IActionResult? RequireLogin()
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToAction("Login", "Auth");
            return null;
        }

        // ── Helper: lấy Id của user đang đăng nhập ─────────────
        private Guid GetCurrentUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
        }

        // ── Helper: build Sidebar ─────────────────────────────────
        private async Task<AccountSidebarViewModel> BuildSidebar(string activeTab)
        {
            var uid = GetCurrentUserId();
            var nd = await _context.NguoiDungs.FindAsync(uid);
            var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaNguoiDung == uid);

            return new AccountSidebarViewModel
            {
                HoTen = nd?.HoTen ?? "",
                SoDienThoai = nd?.SoDienThoai ?? "",
                HangThanhVien = kh?.HangThanhVien ?? "Dong",
                DiemTichLuy = kh?.DiemTichLuy ?? 0,
                ActiveTab = activeTab
            };
        }

        // ============================================================
        // THÔNG TIN CÁ NHÂN
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> ThongTin()
        {
            if (RequireLogin() is { } redirect) return redirect;
            var uid = GetCurrentUserId();
            var nd = await _context.NguoiDungs.FindAsync(uid);
            if (nd == null) return RedirectToAction("Login", "Auth");
            ViewBag.IsGoogleAccount = nd.TenDangNhap.StartsWith("google_");

            var vm = new ThongTinCaNhanViewModel
            {
                Sidebar = await BuildSidebar("thongtin"),
                HoTen = nd.HoTen,
                SoDienThoai = nd.SoDienThoai,
                Email = nd.Email,
                NgaySinh = nd.NgaySinh, // Lưu ý: View Model của bạn cần đổi NgaySinh sang kiểu DateOnly? cho khớp với Model
                GioiTinh = nd.GioiTinh,
                DiaChi = nd.DiaChi
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThongTin(ThongTinCaNhanViewModel vm)
        {
            if (RequireLogin() is { } redirect) return redirect;
            vm.Sidebar = await BuildSidebar("thongtin");
            if (!ModelState.IsValid) return View(vm);

            var uid = GetCurrentUserId();
            var nd = await _context.NguoiDungs.FindAsync(uid);
            if (nd == null) return RedirectToAction("Login", "Auth");

            nd.HoTen = vm.HoTen;
            nd.SoDienThoai = vm.SoDienThoai;
            nd.Email = vm.Email; // Đã fix từ nd.Emai -> nd.Email
            nd.NgaySinh = vm.NgaySinh;
            nd.GioiTinh = vm.GioiTinh;
            nd.DiaChi = vm.DiaChi;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction(nameof(ThongTin));
        }

        // ============================================================
        // ĐỔI MẬT KHẨU
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> DoiMatKhau()
        {
            if (RequireLogin() is { } redirect) return redirect;
            ViewBag.Sidebar = await BuildSidebar("thongtin");
            return View(new DoiMatKhauViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiMatKhau(DoiMatKhauViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMK"] = string.Join("|", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return RedirectToAction(nameof(ThongTin));
            }

            var uid = GetCurrentUserId();
            var nd = await _context.NguoiDungs.FindAsync(uid);
            if (nd == null) return RedirectToAction("Login", "Auth");

            if (!BCrypt.Net.BCrypt.Verify(vm.MatKhauCu, nd.MatKhau))
            {
                TempData["ErrorMK"] = "Mật khẩu hiện tại không đúng";
                return RedirectToAction(nameof(ThongTin));
            }

            nd.MatKhau = BCrypt.Net.BCrypt.HashPassword(vm.MatKhauMoi);
            await _context.SaveChangesAsync();

            TempData["SuccessMK"] = "Đổi mật khẩu thành công!";
            return RedirectToAction(nameof(ThongTin));
        }

        // ============================================================
        // ĐƠN HÀNG CỦA TÔI
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> DonHang(string filter = "TatCa")
        {
            if (RequireLogin() is { } redirect) return redirect;
            var uid = GetCurrentUserId();
            var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaNguoiDung == uid);
            if (kh == null) return RedirectToAction("Login", "Auth");

            var query = _context.DonHangs
                .Where(d => d.MaKhachHang == kh.MaKhachHang);

            if (filter != "TatCa")
                query = query.Where(d => d.TrangThai == filter);

            var donHangs = await query
                .OrderByDescending(d => d.NgayDat)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.Thuoc) // Đã fix từ MaThuocNavigation -> Thuoc
                .ToListAsync();

            var vm = new LichSuDonHangViewModel
            {
                Sidebar = await BuildSidebar("donhang"),
                FilterTrangThai = filter,
                DonHangs = donHangs.Select(d => new DonHangViewModel
                {
                    MaDonHang = d.MaDonHang,
                    NgayDat = d.NgayDat,
                    TrangThai = d.TrangThai,
                    ThanhToan = d.ThanhToan,
                    ChiTiet = d.ChiTietDonHangs.Select(ct => new ChiTietDonHangViewModel
                    {
                        TenThuoc = ct.Thuoc?.TenThuoc ?? "", // Đã fix
                        HinhAnh = ct.Thuoc?.HinhAnh,         // Đã fix
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia
                    }).ToList()
                }).ToList()
            };

            return View(vm);
        }

        // ── Hủy đơn hàng ─────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyDonHang(Guid maDonHang)
        {
            var uid = GetCurrentUserId();
            var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaNguoiDung == uid);
            if (kh == null) return Json(new { success = false });

            var donHang = await _context.DonHangs
                .FirstOrDefaultAsync(d => d.MaDonHang == maDonHang && d.MaKhachHang == kh.MaKhachHang);

            if (donHang == null || donHang.TrangThai != "ChoXacNhan")
                return Json(new { success = false, message = "Không thể hủy đơn hàng này" });

            donHang.TrangThai = "DaHuy";
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // ── Chi tiết đơn hàng (modal) ─────────────────────────────
        [HttpGet]
        public async Task<IActionResult> ChiTietDonHang(Guid maDonHang)
        {
            var uid = GetCurrentUserId();
            var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaNguoiDung == uid);
            if (kh == null) return NotFound();

            var donHang = await _context.DonHangs
                .Include(d => d.DiaChiKhachHang) // Đã fix từ MaDiaChiNavigation -> DiaChiKhachHang
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.Thuoc) // Đã fix từ MaThuocNavigation -> Thuoc
                .FirstOrDefaultAsync(d => d.MaDonHang == maDonHang && d.MaKhachHang == kh.MaKhachHang);

            if (donHang == null) return NotFound();

            return PartialView("_ChiTietDonHangPartial", donHang);
        }

        // ============================================================
        // QUẢN LÝ ĐỊA CHỈ
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> DiaChi()
        {
            if (RequireLogin() is { } redirect) return redirect;
            var uid = GetCurrentUserId();
            var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaNguoiDung == uid);
            if (kh == null) return RedirectToAction("Login", "Auth");

            var diaChis = await _context.DiaChiKhachHangs
                .Where(d => d.MaKhachHang == kh.MaKhachHang)
                .OrderByDescending(d => d.LaMacDinh)
                .ToListAsync();

            var vm = new QuanLyDiaChiViewModel
            {
                Sidebar = await BuildSidebar("diachi"),
                DiaChis = diaChis.Select(d => new DiaChiViewModel
                {
                    MaDiaChi = d.MaDiaChi,
                    HoTenNguoiNhan = d.HoTenNguoiNhan,
                    SdtNguoiNhan = d.SDTNguoiNhan, // Đã fix từ SdtNguoiNhan -> SDTNguoiNhan
                    DiaChi = d.DiaChi,
                    LaMacDinh = d.LaMacDinh
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemDiaChi(ThemDiaChiViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorDC"] = "Vui lòng kiểm tra lại thông tin địa chỉ";
                return RedirectToAction(nameof(DiaChi));
            }

            var uid = GetCurrentUserId();
            var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaNguoiDung == uid);
            if (kh == null) return RedirectToAction("Login", "Auth");

            // Nếu đặt mặc định → bỏ mặc định cũ
            if (vm.LaMacDinh)
            {
                var existing = await _context.DiaChiKhachHangs
                    .Where(d => d.MaKhachHang == kh.MaKhachHang && d.LaMacDinh)
                    .ToListAsync();
                existing.ForEach(d => d.LaMacDinh = false);
            }

            _context.DiaChiKhachHangs.Add(new DiaChiKhachHang
            {
                MaKhachHang = kh.MaKhachHang,
                HoTenNguoiNhan = vm.HoTenNguoiNhan,
                SDTNguoiNhan = vm.SdtNguoiNhan, // Đã fix từ SdtNguoiNhan -> SDTNguoiNhan
                DiaChi = vm.DiaChi,
                LaMacDinh = vm.LaMacDinh
            });

            await _context.SaveChangesAsync();
            TempData["SuccessDC"] = "Thêm địa chỉ thành công!";
            return RedirectToAction(nameof(DiaChi));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaDiaChi(Guid maDiaChi)
        {
            var uid = GetCurrentUserId();
            var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaNguoiDung == uid);
            if (kh == null) return Json(new { success = false });

            var dc = await _context.DiaChiKhachHangs
                .FirstOrDefaultAsync(d => d.MaDiaChi == maDiaChi && d.MaKhachHang == kh.MaKhachHang);

            if (dc == null) return Json(new { success = false });

            _context.DiaChiKhachHangs.Remove(dc);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatMacDinh(Guid maDiaChi)
        {
            var uid = GetCurrentUserId();
            var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaNguoiDung == uid);
            if (kh == null) return Json(new { success = false });

            var all = await _context.DiaChiKhachHangs
                .Where(d => d.MaKhachHang == kh.MaKhachHang)
                .ToListAsync();

            all.ForEach(d => d.LaMacDinh = d.MaDiaChi == maDiaChi);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}