using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class DiaChiKhachHang
    {
        [Key]
        [Column("ma_dia_chi")]
        public Guid MaDiaChi { get; set; }

        [Column("ma_khach_hang")]
        public Guid MaKhachHang { get; set; }

        [Column("ho_ten_nguoi_nhan")]
        [Required(ErrorMessage = "Họ tên người nhận không được để trống")]
        [MaxLength(100)]
        public string HoTenNguoiNhan { get; set; } = string.Empty;

        // Ép kiểu chuẩn varchar cho số điện thoại dưới Database
        [Column("SDT_nguoi_nhan", TypeName = "varchar(15)")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string SDTNguoiNhan { get; set; } = string.Empty;

        [Column("dia_chi")]
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [MaxLength(500)]
        public string DiaChi { get; set; } = string.Empty;

        [Column("la_mac_dinh")]
        public bool LaMacDinh { get; set; }

        // --- NAVIGATION PROPERTIES ---
        // Đã bỏ virtual để dùng Eager Loading

        [ForeignKey("MaKhachHang")]
        public KhachHang KhachHang { get; set; } = null!;

        // 1 Địa chỉ có thể được dùng cho nhiều Đơn hàng (Lấy từ file 1)
        public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
}
