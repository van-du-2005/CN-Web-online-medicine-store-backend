using OnlineMedicineStoreBackend.DTOs.account;
using OnlineMedicineStoreBackend.DTOs.auth; 

namespace OnlineMedicineStoreBackend.Services
{
    public interface IAccountDService
    {
        Task<ApiResponse<UserProfileDto>> GetProfileAsync(Guid userId);
        Task<ApiResponse<string>> UpdateProfileAsync(Guid userId, UserProfileDto dto);
        Task<ApiResponse<string>> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
        Task<ApiResponse<List<OrderHistoryDto>>> GetOrdersAsync(Guid userId, string filter);
        Task<ApiResponse<string>> CancelOrderAsync(Guid userId, Guid orderId);
        Task<ApiResponse<List<AddressDto>>> GetAddressesAsync(Guid userId);
        Task<ApiResponse<string>> AddAddressAsync(Guid userId, AddressDto dto);
        Task<ApiResponse<string>> DeleteAddressAsync(Guid userId, Guid addressId);
        Task<ApiResponse<string>> SetDefaultAddressAsync(Guid userId, Guid addressId);
    }
}