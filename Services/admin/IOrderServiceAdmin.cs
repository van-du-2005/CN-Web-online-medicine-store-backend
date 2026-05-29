using OnlineMedicineStoreBackend.DTOs.admin;
using OnlineMedicineStoreBackend.DTOs.auth; 

namespace OnlineMedicineStoreBackend.Services.admin
{
    public interface IOrderServiceAdmin
    {
        Task<ApiResponse<OrderFilterCountDto>> GetFilterCountsAsync();
        Task<ApiResponse<PagedOrderResultDto>> GetOrdersAsync(string status, int page, int limit);
        Task<ApiResponse<OrderDetailDto>> GetOrderDetailAsync(Guid maDonHang);
        Task<ApiResponse<string>> UpdateOrderStatusAsync(Guid maDonHang, string newStatus, Guid userId);
    }
}