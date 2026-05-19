using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;
namespace OnlineMedicineStoreBackend.Repositories
{
    public class DonHangRepository : GenericRepository<DonHang>, IDonHangRepository
    {
        public DonHangRepository(OnlineMedicineStoreCNWDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DonHang>> GetDonHangByKhachHangAsync(Guid maKhachHang)
        {
            return await _DbSet
                .Where(d => d.MaKhachHang == maKhachHang)
                .OrderByDescending(d => d.NgayDat)
                .ToListAsync();
        }
    }
}