using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.DTOs.admin;

namespace OnlineMedicineStoreBackend.Repositories.admin
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly OnlineMedicineStoreCNWDbContext _context;
        // Xử lý null ảnh theo yêu cầu trước đó
        private const string DEFAULT_IMAGE = "/anh_mi_mac_dinh.png"; 

        public DashboardRepository(OnlineMedicineStoreCNWDbContext context)
        {
            _context = context;
        }

        public async Task<SummaryCardsDto> GetSummaryAsync(DateTime startDate, DateTime endDate)
        {
            var baseOrders = _context.DonHangs
                .Where(d => d.TrangThai == "DaGiao" && d.NgayDat >= startDate && d.NgayDat <= endDate);

            return new SummaryCardsDto
            {
                DoanhThu = await baseOrders.SumAsync(d => (decimal?)d.ThanhToan) ?? 0,
                TongDonHang = await baseOrders.CountAsync(),
                KhachHangMoi = await _context.NguoiDungs
                    .Where(n => n.VaiTro == "KhachHang" && n.NgayTao >= startDate && n.NgayTao <= endDate)
                    .CountAsync(),
                SanPhamDaBan = await _context.ChiTietDonHangs
                    .Where(ct => ct.DonHang.TrangThai == "DaGiao" && ct.DonHang.NgayDat >= startDate && ct.DonHang.NgayDat <= endDate)
                    .SumAsync(ct => (int?)ct.SoLuong) ?? 0
            };
        }

        public async Task<OrderStatusChartDto> GetOrderStatusChartAsync(DateTime startDate, DateTime endDate)
        {
            var stats = await _context.DonHangs
                .Where(d => d.NgayDat >= startDate && d.NgayDat <= endDate)
                .GroupBy(d => d.TrangThai)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var chart = new OrderStatusChartDto();
            string[] allStatuses = { "ChoXacNhan", "DangXuLy", "DangGiao", "DaGiao", "DaHuy" };

            foreach (var status in allStatuses)
            {
                chart.Labels.Add(GetStatusLabel(status));
                chart.Data.Add(stats.FirstOrDefault(s => s.Status == status)?.Count ?? 0);
            }
            return chart;
        }

        public async Task<(List<TopProductDto> Items, int TotalCount)> GetTopProductsAsync(DateTime startDate, DateTime endDate, int page, int limit)
        {
            var query = _context.ChiTietDonHangs
                .Include(ct => ct.Thuoc)
                .Where(ct => ct.DonHang.TrangThai == "DaGiao" && ct.DonHang.NgayDat >= startDate && ct.DonHang.NgayDat <= endDate && ct.Thuoc.TrangThai == true)
                .GroupBy(ct => new { ct.Thuoc.MaThuoc, ct.Thuoc.TenThuoc, ct.Thuoc.HinhAnh })
                .Select(g => new TopProductDto
                {
                    MaThuoc = g.Key.MaThuoc,
                    TenThuoc = g.Key.TenThuoc,
                    HinhAnh = !string.IsNullOrEmpty(g.Key.HinhAnh) ? g.Key.HinhAnh : DEFAULT_IMAGE,
                    SoLuongBan = g.Sum(x => x.SoLuong),
                    DoanhThu = g.Sum(x => x.ThanhTien) ?? 0m
                });

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.SoLuongBan).ThenByDescending(x => x.DoanhThu)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            // Cập nhật STT cho đúng với Page hiện tại
            for (int i = 0; i < items.Count; i++) items[i].STT = (page - 1) * limit + i + 1;

            return (items, totalCount);
        }

        public async Task<(List<TopCustomerDto> Items, int TotalCount)> GetTopCustomersAsync(DateTime startDate, DateTime endDate, int page, int limit)
        {
            var query = _context.DonHangs
                .Include(d => d.KhachHang).ThenInclude(k => k.NguoiDung)
                .Where(d => d.TrangThai == "DaGiao" && d.NgayDat >= startDate && d.NgayDat <= endDate)
                .GroupBy(d => new { d.MaKhachHang, d.KhachHang.NguoiDung.HoTen })
                .Select(g => new TopCustomerDto
                {
                    HoTen = g.Key.HoTen ?? "Khách vãng lai",
                    SoDon = g.Count(),
                    TongTien = g.Sum(x => x.ThanhToan)
                });

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.TongTien).ThenByDescending(x => x.SoDon)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            for (int i = 0; i < items.Count; i++) items[i].STT = (page - 1) * limit + i + 1;

            return (items, totalCount);
        }

        public async Task<(List<LowStockDto> Items, int TotalCount)> GetLowStockProductsAsync(int page, int limit)
        {
            var query = _context.Thuocs.Where(t => t.TrangThai == true && t.TonKhoHienTai < 10);
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(t => t.TonKhoHienTai)
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(t => new LowStockDto
                {
                    MaThuoc = t.MaThuoc,
                    TenThuoc = t.TenThuoc,
                    TonKhoHienTai = t.TonKhoHienTai,
                    HinhAnh = !string.IsNullOrEmpty(t.HinhAnh) ? t.HinhAnh : DEFAULT_IMAGE
                })
                .ToListAsync();

            return (items, totalCount);
        }

        private string GetStatusLabel(string status) => status switch
        {
            "ChoXacNhan" => "Chờ xác nhận",
            "DangXuLy" => "Đang xử lý",
            "DangGiao" => "Đang giao",
            "DaGiao" => "Đã giao",
            "DaHuy" => "Đã hủy",
            _ => "Khác"
        };
    }
}