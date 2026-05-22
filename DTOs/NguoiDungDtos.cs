using System.ComponentModel.DataAnnotations;

namespace OnlineMedicineStoreBackend.DTOs
{
    // 1. DTO dùng để trả dữ liệu về cho Angular (Khớp 100% với Frontend)
    public class NguoiDungDto
    {
        public Guid Id { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Sdt { get; set; }
        public string VaiTro { get; set; } = string.Empty;
        public bool TrangThai { get; set; }
    }

    // 2. DTO dùng để nhận dữ liệu từ Form Thêm Mới
    public class TaoNguoiDungDto
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        public string Sdt { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string MatKhau { get; set; } = string.Empty;

        public string VaiTro { get; set; } = "KhachHang";
    }

    // 3. DTO dùng để nhận dữ liệu từ Form Cập Nhật
    public class CapNhatNguoiDungDto
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        public string Sdt { get; set; } = string.Empty;

        public string VaiTro { get; set; } = string.Empty;
    }
}