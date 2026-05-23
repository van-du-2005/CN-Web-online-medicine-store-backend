using OnlineMedicineStoreBackend.DTOs.admin;

namespace OnlineMedicineStoreBackend.Repositories.admin
{
    public interface IDashboardRepository
    {
        Task<SummaryCardsDto> GetSummaryAsync(DateTime startDate, DateTime endDate);
        Task<OrderStatusChartDto> GetOrderStatusChartAsync(DateTime startDate, DateTime endDate);
        // Hỗ trợ Lazy Loading (page, limit)
        Task<(List<TopProductDto> Items, int TotalCount)> GetTopProductsAsync(DateTime startDate, DateTime endDate, int page, int limit);
        Task<(List<TopCustomerDto> Items, int TotalCount)> GetTopCustomersAsync(DateTime startDate, DateTime endDate, int page, int limit);
        Task<(List<LowStockDto> Items, int TotalCount)> GetLowStockProductsAsync(int page, int limit);
    }
}