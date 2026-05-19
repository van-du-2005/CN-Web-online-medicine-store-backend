using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class ChiTietDonHang
    {
        [Key]
        [Column("ma_chi_tiet")]
        public Guid MaChiTiet { get; set; }

        [Column("ma_don_hang")]
        public Guid MaDonHang { get; set; }

        [Column("ma_thuoc")]
        public Guid MaThuoc { get; set; }

        [Column("ma_lo")]
        public Guid MaLo { get; set; }

        [Column("so_luong")]
        public int SoLuong { get; set; }

        [Column("don_gia", TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }

        [Column("thanh_tien", TypeName = "decimal(18,2)")]
        public decimal? ThanhTien { get; set; }

        // --- NAVIGATION PROPERTIES ---
        // Đã bỏ virtual để dùng Eager Loading (.Include)

        [ForeignKey("MaDonHang")]
        public DonHang DonHang { get; set; } = null!;

        [ForeignKey("MaThuoc")]
        public Thuoc Thuoc { get; set; } = null!;

        [ForeignKey("MaLo")]
        public LoThuoc LoThuoc { get; set; } = null!;
    }
}
