namespace OnlineMedicineStoreBackend.DTOs.admin
{
    // Class dùng chung để trả về dữ liệu phân trang (Lazy Loading)
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }

    public class SummaryCardsDto
    {
        public decimal DoanhThu { get; set; }
        public int TongDonHang { get; set; }
        public int KhachHangMoi { get; set; }
        public int SanPhamDaBan { get; set; }
    }

    public class OrderStatusChartDto
    {
        public List<string> Labels { get; set; } = new List<string>();
        public List<int> Data { get; set; } = new List<int>();
    }

    public class TopProductDto
    {
        public int STT { get; set; }
        public Guid MaThuoc { get; set; }
        public string TenThuoc { get; set; } = string.Empty;
        public string HinhAnh { get; set; } = string.Empty;
        public int SoLuongBan { get; set; }
        public decimal DoanhThu { get; set; }
    }

    public class TopCustomerDto
    {
        public int STT { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public int SoDon { get; set; }
        public decimal TongTien { get; set; }
    }

    public class LowStockDto
    {
        public Guid MaThuoc { get; set; }
        public string TenThuoc { get; set; } = string.Empty;
        public int TonKhoHienTai { get; set; }
        public string HinhAnh { get; set; } = string.Empty;
    }
}