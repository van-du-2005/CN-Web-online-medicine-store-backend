namespace OnlineMedicineStoreBackend.DTOs.account
{
    public class UserProfileDto
    {
        public string HoTen { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public DateOnly? NgaySinh { get; set; }
        public bool? GioiTinh { get; set; }
        public string? DiaChi { get; set; }
       
        public string HangThanhVien { get; set; } = string.Empty;
        public int DiemTichLuy { get; set; }
    }
}