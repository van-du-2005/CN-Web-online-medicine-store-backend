namespace OnlineMedicineStoreBackend.DTOs
{
    public class CartDto
    {
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();

        public decimal TotalPrice => Items.Sum(item => item.TongTien); // Tính tổng giá trị của giỏ hàng
        public int TotalQuantity => Items.Sum(item => item.SoLuong); // Tính tổng số lượng mặt hàng trong giỏ hàng
    }
}