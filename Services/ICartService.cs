using OnlineMedicineStoreBackend.DTOs;

namespace OnlineMedicineStoreBackend.Services
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(Guid userId);
        Task<bool> AddCartAsync(Guid userId, Guid maThuoc, int soLuong);
        Task<bool> UpdateCartItemQuantityAsync(Guid userId, Guid maThuoc, int soLuong);
        Task<bool> RemoveCartItemAsync(Guid userId, Guid maThuoc);
    }
}