using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class ChiTietGioHang
    {
        [Key]
        [Column("ma_chi_tiet")]
        public Guid MaChiTiet { get; set; }

        [Column("ma_gio_hang")]
        public Guid MaGioHang { get; set; }

        [Column("ma_thuoc")]
        public Guid MaThuoc { get; set; }

        [Column("so_luong")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        [Column("ngay_them")]
        public DateTime NgayThem { get; set; }

        // --- NAVIGATION PROPERTIES ---

        [ForeignKey("MaGioHang")]
        public GioHang GioHang { get; set; } = null!;

        [ForeignKey("MaThuoc")]
        public Thuoc Thuoc { get; set; } = null!;
    }
}
