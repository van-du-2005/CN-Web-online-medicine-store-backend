namespace OnlineMedicineStoreBackend.DTOs
{
    public class CartItemDto
    {
        public Guid MaThuoc { get; set; }
        public string TenThuoc { get; set; } = string.Empty;
        public string HinhAnh { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
        public decimal TongTien => SoLuong * Gia; // Tính tổng tiền cho mỗi mặt hàng
    }
}