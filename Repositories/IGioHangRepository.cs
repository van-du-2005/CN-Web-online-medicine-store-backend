using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories
{
    public interface IGioHangRepository : IGenericRepository<GioHang>
    {
        Task<GioHang?> GetCartWithDetailsAsync(Guid userId);
        Task AddCartItemAsync(Guid userId, Guid maThuoc, int soLuong);
        Task UpdateCartItemQuantityAsync(Guid userId, Guid maThuoc, int soLuong);
        Task RemoveCartItemAsync(Guid userId, Guid maThuoc);
    }
}