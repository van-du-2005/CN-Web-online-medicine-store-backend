namespace OnlineMedicineStoreBackend.DTOs
{
    public class OrderDTO
    {
        public Guid MaDonHang { get; set; }
        public DateTime NgayDat { get; set; }
        public Decimal TongTienThanhToan { get; set; }
        public string TrangThai {  get; set; } = string.Empty;
        public string PhuongThucThanhToan { get; set; } = string.Empty;
    }
}