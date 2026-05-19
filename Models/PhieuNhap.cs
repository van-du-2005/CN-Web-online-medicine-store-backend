
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class PhieuNhap
    {
        [Key]
        [Column("ma_phieu_nhap")]

        public Guid MaPhieuNhap { get; set; }

        [Column("ma_nhan_vien")]
        public Guid MaNhanVien { get; set; }

        [Column("ma_nha_cung_cap")]
        public Guid MaNhaCungCap { get; set; }

        [Column("ngay_nhap")]
        public DateTime NgayNhap { get; set; }

        // Cho phép null, và giữ nguyên định dạng decimal(18,2) cho database
        [Column("tong_tien", TypeName = "decimal(18,2)")]
        public decimal? TongTien { get; set; }

        [Column("ghi_chu")]
        public string? GhiChu { get; set; }

        [Column("trang_thai")]
        public string TrangThai { get; set; } = string.Empty;

        // --- NAVIGATION PROPERTIES ---

        [ForeignKey("MaNhanVien")]
        public NhanVien NhanVien { get; set; } = null!;

        [ForeignKey("MaNhaCungCap")]
        public NhaCungCap NhaCungCap { get; set; } = null!;

        // 1 Phiếu nhập chứa nhiều Lô thuốc
        public ICollection<LoThuoc> LoThuocs { get; set; } = new List<LoThuoc>();
    }
}
