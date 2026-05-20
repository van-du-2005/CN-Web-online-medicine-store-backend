namespace OnlineMedicineStoreBackend.DTOs
{
    public class CheckoutDTO
    {
        public Guid MaKhachHang { get; set; }
        public string TenNguoiMua { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string Tinh { get; set; } = string.Empty;
        public string Phuong { get; set; } = string.Empty;
        public string DiaChiCuThe { get; set; } = string.Empty;
        public decimal TongTienThanhToan { get; set; }
        public string PhuongThucThanhToan { get; set; } = string.Empty;

        public List<OrderItemDTO> SanPhamDaMua { get; set; } = new List<OrderItemDTO>();
    }
}