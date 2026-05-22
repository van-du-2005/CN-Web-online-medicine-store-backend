using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories.admin
{
    public class OrderRepositoryAdmin : IOrderRepositoryAdmin
    {
        private readonly OnlineMedicineStoreCNWDbContext _context;
        public OrderRepositoryAdmin(OnlineMedicineStoreCNWDbContext context) => _context = context;

        public async Task<Dictionary<string, int>> GetOrderCountsAsync()
        {
            return await _context.DonHangs
                .GroupBy(d => d.TrangThai)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        public async Task<(List<DonHang> Items, int TotalCount)> GetPagedOrdersAsync(string status, int page, int limit)
        {
            var query = _context.DonHangs
                .Include(d => d.KhachHang).ThenInclude(k => k.NguoiDung)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "TatCa")
                query = query.Where(d => d.TrangThai == status);

            int totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(d => d.NgayDat)
                .Skip((page - 1) * limit).Take(limit)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<DonHang?> GetOrderDetailAsync(Guid maDonHang)
        {
            return await _context.DonHangs
                .Include(d => d.KhachHang).ThenInclude(k => k.NguoiDung)
                .Include(d => d.DiaChiKhachHang)
                .Include(d => d.ChiTietDonHangs).ThenInclude(ct => ct.Thuoc)
                .FirstOrDefaultAsync(d => d.MaDonHang == maDonHang);
        }

        public async Task<DonHang?> GetOrderByIdAsync(Guid maDonHang)
        {
            return await _context.DonHangs.FirstOrDefaultAsync(d => d.MaDonHang == maDonHang);
        }

        public async Task<NhanVien?> GetNhanVienByUserIdAsync(Guid userId)
        {
            return await _context.NhanViens.FirstOrDefaultAsync(nv => nv.MaNguoiDung == userId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}