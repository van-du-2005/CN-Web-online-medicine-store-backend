namespace OnlineMedicineStoreBackend.DTOs.admin
{
    public class OrderFilterCountDto
    {
        public int TatCa { get; set; }
        public int ChoXacNhan { get; set; }
        public int DangXuLy { get; set; }
        public int DangGiao { get; set; }
        public int DaGiao { get; set; }
        public int DaHuy { get; set; }
    }

    public class OrderListItemDto
    {
        public Guid MaDonHang { get; set; }
        public string MaDonCode => $"DH-{MaDonHang.ToString().Substring(0, 8).ToUpper()}"; 
        public string TenKhachHang { get; set; } = string.Empty;
        public string NgayDatFormat { get; set; } = string.Empty;
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public string TrangThaiLabel { get; set; } = string.Empty;
        public string ColorCode { get; set; } = string.Empty;
    }

    public class OrderDetailDto
    {
        public Guid MaDonHang { get; set; }
        public string MaDonCode => $"DH-{MaDonHang.ToString().Substring(0, 8).ToUpper()}";
        public string NgayDatFormat { get; set; } = string.Empty;

        // Khách hàng
        public string HoTen { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;

        // Trạng thái & Thanh toán
        public string TrangThai { get; set; } = string.Empty;
        public string PhuongThucThanhToan { get; set; } = string.Empty;

        // Tiền nong
        public decimal TamTinh { get; set; }
        public decimal PhiVanChuyen { get; set; }
        public decimal GiamGia { get; set; }
        public decimal TongTien { get; set; }

        public List<OrderItemDto> SanPhams { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int STT { get; set; }
        public string TenThuoc { get; set; } = string.Empty;
        public string HinhAnh { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; }
    }
    
    // Class dùng  để trả về dữ liệu phân trang
    public class PagedOrderResultDto
    {
        public List<OrderListItemDto> Items { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}