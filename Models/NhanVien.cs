using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class NhanVien
    {
        [Key]
        [Column("ma_nhan_vien")]
        public Guid MaNhanVien { get; set; }

        [Column("ma_nguoi_dung")]
        public Guid MaNguoiDung { get; set; }

        [Column("chuc_vu")]
        [Required(ErrorMessage = "Chức vụ không được để trống")]
        [MaxLength(100)]
        public string ChucVu { get; set; } = string.Empty;

        [Column("ngay_vao_lam")]
        public DateOnly? NgayVaoLam { get; set; }

        [Column("luong", TypeName = "decimal(18,2)")]
        public decimal? Luong { get; set; }

        // --- NAVIGATION PROPERTIES ---

        [ForeignKey("MaNguoiDung")]
        public NguoiDung NguoiDung { get; set; } = null!;

        // 1 Nhân viên có thể xử lý nhiều Đơn hàng
        public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

        // 1 Nhân viên có thể tạo/xử lý nhiều Phiếu nhập kho
        public ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();
    }
}