
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class NhaCungCap
    {
        [Key]
        [Column("ma_nha_cung_cap")]
        public Guid MaNhaCungCap { get; set; }


        [Required(ErrorMessage = "Tên công ty không được để trống")]
        [StringLength(255)]
        [Column("ten_cong_ty")]
        public string TenCongTy { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("nguoi_lien_he")]
        public string? NguoiLienHe { get; set; }

        // Ép kiểu varchar cho số điện thoại
        [Column("so_dien_thoai", TypeName = "varchar(20)")]
        public string? SoDienThoai { get; set; }

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Column("email")]
        public string? Email { get; set; }

        [Column("dia_chi")]
        public string? DiaChi { get; set; }

        [Column("trang_thai")]
        public bool TrangThai { get; set; } = true;

        // --- NAVIGATION PROPERTIES ---

        // 1 Nhà cung cấp có nhiều Phiếu nhập
        public ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();

        // 1 Nhà cung cấp có thể cung cấp nhiều Lô thuốc
        public ICollection<LoThuoc> LoThuocs { get; set; } = new List<LoThuoc>();
    }
}
