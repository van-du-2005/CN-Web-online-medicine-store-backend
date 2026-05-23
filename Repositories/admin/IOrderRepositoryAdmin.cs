using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories.admin
{
    public interface IOrderRepositoryAdmin
    {
        Task<Dictionary<string, int>> GetOrderCountsAsync();
        Task<(List<DonHang> Items, int TotalCount)> GetPagedOrdersAsync(string status, int page, int limit);
        Task<DonHang?> GetOrderDetailAsync(Guid maDonHang);
        Task<DonHang?> GetOrderByIdAsync(Guid maDonHang); 
        Task<NhanVien?> GetNhanVienByUserIdAsync(Guid userId);
        Task SaveChangesAsync();
    }
}