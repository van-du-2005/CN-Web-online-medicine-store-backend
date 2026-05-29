using OnlineMedicineStoreBackend.DTOs.admin;
using OnlineMedicineStoreBackend.DTOs.auth; 
namespace OnlineMedicineStoreBackend.Services.admin
{
    public interface IDashboardService
    {
        Task<ApiResponse<SummaryCardsDto>> GetSummaryAsync(string filter);
        Task<ApiResponse<OrderStatusChartDto>> GetChartAsync(string filter);
        Task<ApiResponse<PagedResultDto<TopProductDto>>> GetTopProductsAsync(string filter, int page, int limit);
        Task<ApiResponse<PagedResultDto<TopCustomerDto>>> GetTopCustomersAsync(string filter, int page, int limit);
        Task<ApiResponse<PagedResultDto<LowStockDto>>> GetLowStockProductsAsync(int page, int limit);
    }
}