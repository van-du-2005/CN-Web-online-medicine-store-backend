using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories
{
    public class AccountDRepository : IAccountDRepository
    {
        private readonly OnlineMedicineStoreCNWDbContext _context;

        public AccountDRepository(OnlineMedicineStoreCNWDbContext context)
        {
            _context = context;
        }

        public async Task<NguoiDung?> GetUserWithCustomerInfoAsync(Guid userId)
        {
            return await _context.NguoiDungs
                .Include(u => u.KhachHang)
                .FirstOrDefaultAsync(u => u.MaNguoiDung == userId);
        }

        public async Task<KhachHang?> GetCustomerByUserIdAsync(Guid userId)
        {
            return await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaNguoiDung == userId);
        }

        public async Task<List<DonHang>> GetOrdersByCustomerIdAsync(Guid customerId, string filter)
        {
            var query = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.Thuoc)
                .Where(d => d.MaKhachHang == customerId);

            if (filter != "TatCa") query = query.Where(d => d.TrangThai == filter);

            return await query.OrderByDescending(d => d.NgayDat).ToListAsync();
        }

        public async Task<DonHang?> GetOrderByIdAsync(Guid orderId, Guid customerId)
        {
            return await _context.DonHangs
                .Include(d => d.DiaChiKhachHang)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.Thuoc)
                .FirstOrDefaultAsync(d => d.MaDonHang == orderId && d.MaKhachHang == customerId);
        }

        public async Task<List<DiaChiKhachHang>> GetAddressesAsync(Guid customerId)
        {
            return await _context.DiaChiKhachHangs
                .Where(d => d.MaKhachHang == customerId)
                .OrderByDescending(d => d.LaMacDinh)
                .ToListAsync();
        }

        public async Task<DiaChiKhachHang?> GetAddressByIdAsync(Guid addressId, Guid customerId)
        {
            return await _context.DiaChiKhachHangs
                .FirstOrDefaultAsync(d => d.MaDiaChi == addressId && d.MaKhachHang == customerId);
        }

        public async Task AddAddressAsync(DiaChiKhachHang address)
        {
            await _context.DiaChiKhachHangs.AddAsync(address);
        }

        public async Task DeleteAddressAsync(DiaChiKhachHang address)
        {
            _context.DiaChiKhachHangs.Remove(address);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}