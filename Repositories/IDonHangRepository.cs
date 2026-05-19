using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories
{
    public interface IDonHangRepository: IGenericRepository<DonHang>
    {
        Task<IEnumerable<DonHang>> GetDonHangByKhachHangAsync(Guid maKhachHang);
    }
}