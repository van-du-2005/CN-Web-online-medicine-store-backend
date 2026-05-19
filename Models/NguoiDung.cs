using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class NguoiDung
    {
        [Key]
        [Column("ma_nguoi_dung")]
        public Guid MaNguoiDung { get; set; }

        [Column("ten_dang_nhap")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [MaxLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Column("mat_khau")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MaxLength(255)]
        public string MatKhau { get; set; } = string.Empty;

        [Column("ho_ten")]
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [MaxLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Column("email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [MaxLength(100)]
        public string? Email { get; set; }

        // Ép kiểu chuẩn varchar cho số điện thoại dưới DB
        [Column("so_dien_thoai", TypeName = "varchar(15)")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string? SoDienThoai { get; set; }

        [Column("ngay_sinh")]
        public DateOnly? NgaySinh { get; set; }

        [Column("gioi_tinh")]
        public bool? GioiTinh { get; set; }

        [Column("dia_chi")]
        [MaxLength(255)]
        public string? DiaChi { get; set; }

        [Column("vai_tro")]
        [MaxLength(50)]
        public string VaiTro { get; set; } = "KhachHang";

        [Column("ngay_tao")]
        public DateTime NgayTao { get; set; }

        [Column("trang_thai")]
        public bool TrangThai { get; set; } = true;

        // --- NAVIGATION PROPERTIES ---

        public NhanVien? NhanVien { get; set; }
        public KhachHang? KhachHang { get; set; }
    }
}
