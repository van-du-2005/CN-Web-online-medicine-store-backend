namespace OnlineMedicineStoreBackend.DTOs.account
{
    public class OrderHistoryDto
    {
        public Guid MaDonHang { get; set; }
        public DateTime NgayDat { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public decimal TamTinh { get; set; }
        public decimal PhiVanChuyen { get; set; }
        public decimal GiamGia { get; set; }
        public decimal ThanhToan { get; set; }
        public List<OrderDetailDto> ChiTiet { get; set; } = new();
    }

    public class OrderDetailDto
    {
        public string TenThuoc { get; set; } = string.Empty;
        public string? HinhAnh { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }
}