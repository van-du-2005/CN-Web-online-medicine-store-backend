using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class DonHang
    {
        [Key]
        [Column("ma_don_hang")]
        public Guid MaDonHang { get; set; }

        [Column("ma_khach_hang")]
        public Guid MaKhachHang { get; set; }

        // Cho phép null vì có thể khách mua tại quầy không cần địa chỉ giao hàng
        [Column("ma_dia_chi")]
        public Guid? MaDiaChi { get; set; }

        // Cho phép null vì đơn hàng online có thể chưa có nhân viên tiếp nhận ngay
        [Column("ma_nhan_vien")]
        public Guid? MaNhanVien { get; set; }

        [Column("ngay_dat")]
        public DateTime NgayDat { get; set; }

        [Column("loai_don")]
        public string LoaiDon { get; set; } = string.Empty;

        [Column("tam_tinh", TypeName = "decimal(18,2)")]
        public decimal TamTinh { get; set; }

        [Column("phi_van_chuyen", TypeName = "decimal(18,2)")]
        public decimal PhiVanChuyen { get; set; }

        [Column("giam_gia", TypeName = "decimal(18,2)")]
        public decimal GiamGia { get; set; }

        [Column("thanh_toan", TypeName = "decimal(18,2)")]
        public decimal ThanhToan { get; set; }

        [Column("phuong_thuc_TT")]
        public string? PhuongThucThanhToan { get; set; } // Nullable theo cấu trúc file 2

        [Column("trang_thai")]
        public string TrangThai { get; set; } = string.Empty;

        // --- NAVIGATION PROPERTIES ---

        [ForeignKey("MaKhachHang")]
        public KhachHang KhachHang { get; set; } = null!;

        [ForeignKey("MaDiaChi")]
        public DiaChiKhachHang? DiaChiKhachHang { get; set; } // Thêm ? vì MaDiaChi là Guid?

        [ForeignKey("MaNhanVien")]
        public NhanVien? NhanVien { get; set; } // Thêm ? vì MaNhanVien là Guid?

        // Quan hệ 1 - N: 1 Đơn hàng có nhiều Chi tiết đơn hàng
        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    }
}
