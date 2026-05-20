namespace OnlineMedicineStoreBackend.DTOs
{
    public class OrderItemDTO
    {
        public Guid MaThuoc { get; set; }
        public string TenThuoc { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal TongTien { get; set; }
    }
}