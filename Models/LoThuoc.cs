using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class LoThuoc
    {
        [Key]
        [Column("ma_lo")]
        public Guid MaLo { get; set; }

        [Column("ma_thuoc")]
        public Guid MaThuoc { get; set; }

        [Column("ma_phieu_nhap")]
        public Guid MaPhieuNhap { get; set; }

        [Column("ma_nha_cung_cap")]
        public Guid MaNhaCungCap { get; set; }

        [Column("ngay_san_xuat")]
        public DateOnly? NgaySanXuat { get; set; }

        [Column("ngay_het_han")]
        public DateOnly NgayHetHan { get; set; }

        [Column("so_luong_nhap")]
        public int SoLuongNhap { get; set; }

        [Column("so_luong_hien_tai")]
        public int SoLuongHienTai { get; set; }

        [Column("gia_nhap", TypeName = "decimal(18,2)")]
        public decimal GiaNhap { get; set; }

        // --- NAVIGATION PROPERTIES ---
        // Đã bỏ virtual

        [ForeignKey("MaThuoc")]
        public Thuoc Thuoc { get; set; } = null!;

        [ForeignKey("MaPhieuNhap")]
        public PhieuNhap PhieuNhap { get; set; } = null!;

        [ForeignKey("MaNhaCungCap")]
        public NhaCungCap NhaCungCap { get; set; } = null!;

        // 1 Lô thuốc có thể xuất hiện trong nhiều Chi tiết đơn hàng
        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    }
}
