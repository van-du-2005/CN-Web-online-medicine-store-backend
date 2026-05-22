using OnlineMedicineStoreBackend.DTOs.admin;
using OnlineMedicineStoreBackend.DTOs.auth;
using OnlineMedicineStoreBackend.Repositories.admin;

namespace OnlineMedicineStoreBackend.Services.admin
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;

        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }

       
        private (DateTime Start, DateTime End) GetDateRange(string filter)
        {
            var now = DateTime.Now;
            return filter?.ToLower() switch
            {
                "homnay" => (now.Date, now.Date.AddDays(1).AddTicks(-1)),
                "tuannay" => (now.AddDays(-1 * ((7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7)).Date, 
                              now.AddDays(-1 * ((7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7)).Date.AddDays(7).AddTicks(-1)),
                "thangnay" => (new DateTime(now.Year, now.Month, 1), 
                               new DateTime(now.Year, now.Month, 1).AddMonths(1).AddTicks(-1)),
                "namnay" => (new DateTime(now.Year, 1, 1), new DateTime(now.Year, 1, 1).AddYears(1).AddTicks(-1)),
                _ => (new DateTime(now.Year, now.Month, 1), new DateTime(now.Year, now.Month, 1).AddMonths(1).AddTicks(-1)) // Mặc định tháng này
            };
        }

        public async Task<ApiResponse<SummaryCardsDto>> GetSummaryAsync(string filter)
        {
            var range = GetDateRange(filter);
            var data = await _repo.GetSummaryAsync(range.Start, range.End);
            return new ApiResponse<SummaryCardsDto> { Success = true, Message = "Lấy dữ liệu thành công.", Data = data };
        }

        public async Task<ApiResponse<OrderStatusChartDto>> GetChartAsync(string filter)
        {
            var range = GetDateRange(filter);
            var data = await _repo.GetOrderStatusChartAsync(range.Start, range.End);
            return new ApiResponse<OrderStatusChartDto> { Success = true, Message = "Lấy dữ liệu thành công.", Data = data };
        }

        public async Task<ApiResponse<PagedResultDto<TopProductDto>>> GetTopProductsAsync(string filter, int page, int limit)
        {
            if (page < 1) page = 1;
            if (limit <= 0 || limit > 50) limit = 10; 
            var range = GetDateRange(filter);
            var (items, totalCount) = await _repo.GetTopProductsAsync(range.Start, range.End, page, limit);

            var result = new PagedResultDto<TopProductDto>
            {
                Items = items,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)limit),
                TotalItems = totalCount
            };
            return new ApiResponse<PagedResultDto<TopProductDto>> { Success = true, Message = "Thành công.", Data = result };
        }

        public async Task<ApiResponse<PagedResultDto<TopCustomerDto>>> GetTopCustomersAsync(string filter, int page, int limit)
        {
            if (page < 1) page = 1;
            if (limit <= 0 || limit > 50) limit = 10;

            var range = GetDateRange(filter);
            var (items, totalCount) = await _repo.GetTopCustomersAsync(range.Start, range.End, page, limit);

            var result = new PagedResultDto<TopCustomerDto>
            {
                Items = items,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)limit),
                TotalItems = totalCount
            };
            return new ApiResponse<PagedResultDto<TopCustomerDto>> { Success = true, Message = "Thành công.", Data = result };
        }

        public async Task<ApiResponse<PagedResultDto<LowStockDto>>> GetLowStockProductsAsync(int page, int limit)
        {
            if (page < 1) page = 1;
            if (limit <= 0 || limit > 50) limit = 15;

            var (items, totalCount) = await _repo.GetLowStockProductsAsync(page, limit);

            var result = new PagedResultDto<LowStockDto>
            {
                Items = items,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)limit),
                TotalItems = totalCount
            };
            return new ApiResponse<PagedResultDto<LowStockDto>> { Success = true, Message = "Thành công.", Data = result };
        }
    }
}