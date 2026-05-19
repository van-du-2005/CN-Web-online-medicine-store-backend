using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class KhachHang
    {
        [Key]
        [Column("ma_khach_hang")]
        public Guid MaKhachHang { get; set; }

        [Column("ma_nguoi_dung")]
        public Guid MaNguoiDung { get; set; }

        [Column("diem_tich_luy")]
        public int DiemTichLuy { get; set; }

        [Column("hang_thanh_vien")]
        [MaxLength(20)]
        public string HangThanhVien { get; set; } = string.Empty;

        // --- NAVIGATION PROPERTIES ---

        [ForeignKey("MaNguoiDung")]
        public NguoiDung NguoiDung { get; set; } = null!;

        // 1 Khách hàng có 1 Giỏ hàng (Cho phép null nếu chưa tạo giỏ)
        public GioHang? GioHang { get; set; }

        // 1 Khách hàng có nhiều Địa chỉ
        public ICollection<DiaChiKhachHang> DiaChiKhachHangs { get; set; } = new List<DiaChiKhachHang>();

        // 1 Khách hàng có nhiều Đơn hàng
        public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
}
