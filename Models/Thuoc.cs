using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class Thuoc
    {
        [Key]
        [Column("ma_thuoc")]
        public Guid MaThuoc { get; set; }

        [Column("ma_danh_muc")]
        public Guid MaDanhMuc { get; set; }

        [Column("ten_thuoc")]
        [Required(ErrorMessage = "Tên thuốc không được để trống")]
        [MaxLength(255)]
        public string TenThuoc { get; set; } = string.Empty;

        [Column("ten_khoa_hoc")]
        [MaxLength(255)]
        public string? TenKhoaHoc { get; set; }

        [Column("loai_thuoc")]
        [MaxLength(100)]
        public string? LoaiThuoc { get; set; }

        // Đồng bộ kiểu decimal(18,2) với các bảng khác để tính tiền chuẩn xác
        [Column("gia_ban", TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn hoặc bằng 0")]
        public decimal GiaBan { get; set; }

        [Column("mo_ta")]
        public string? MoTa { get; set; }

        [Column("huong_dan_su_dung")]
        public string? HuongDanSuDung { get; set; }

        [Column("ton_kho_hien_tai")]
        [Range(0, int.MaxValue, ErrorMessage = "Tồn kho không được là số âm")]
        public int TonKhoHienTai { get; set; } = 0;

        [Column("hinh_anh")]
        [MaxLength(500)]
        public string? HinhAnh { get; set; }

        [Column("trang_thai")]
        public bool TrangThai { get; set; } = true;

        // --- NAVIGATION PROPERTIES ---

        [ForeignKey("MaDanhMuc")]
        public DanhMuc DanhMuc { get; set; } = null!;
        // 1 Loại thuốc có thể xuất hiện trong nhiều Chi tiết đơn hàng
        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

        // 1 Loại thuốc có thể nằm trong nhiều Giỏ hàng
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
        public ICollection<LoThuoc> LoThuocs { get; set; } = new List<LoThuoc>();
    }
}