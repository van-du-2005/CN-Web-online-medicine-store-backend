using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class GioHang
    {
        [Key]
        [Column("ma_gio_hang")]
        public Guid MaGioHang { get; set; }

        [Column("ma_khach_hang")]
        public Guid MaKhachHang { get; set; }

        [Column("ngay_tao")]
        public DateTime NgayTao { get; set; }

        // --- NAVIGATION PROPERTIES ---
        [ForeignKey("MaKhachHang")]
        public KhachHang KhachHang { get; set; } = null!;

        // Danh sách các món hàng trong giỏ (Quan hệ 1 - N với ChiTietGioHang)
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
    }
}
