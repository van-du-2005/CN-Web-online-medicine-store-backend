using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMedicineStoreBackend.Models
{
    public class DanhMuc
    {
        [Key]
        [Column("ma_danh_muc")]
        public Guid MaDanhMuc { get; set; }

        [Column("ten_danh_muc")]
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [MaxLength(255)]
        public string TenDanhMuc { get; set; } = string.Empty;

        [Column("mo_ta")]
        public string? MoTa { get; set; }

        // --- 2 CỘT BỔ SUNG ĐỂ LÀM DANH MỤC CHA CON ---
        [Column("thu_tu")]
        public int ThuTu { get; set; } = 0; // Lưu thứ tự sắp xếp

        [Column("danh_muc_cha_id")]
        public Guid? DanhMucChaId { get; set; } // Dùng Guid? (có dấu ?) vì danh mục chính sẽ để NULL
        // ---------------------------------------------

        [Column("trang_thai")]
        public bool TrangThai { get; set; } = true;

        // --- NAVIGATION PROPERTIES ---
        // Quan hệ 1 - N: 1 Danh mục có nhiều Thuốc
        public ICollection<Thuoc> Thuocs { get; set; } = new List<Thuoc>();

        // Thiết lập quan hệ Đệ quy (Cha - Con) trong cùng 1 bảng
        [ForeignKey("DanhMucChaId")]
        public DanhMuc? DanhMucCha { get; set; }
        public ICollection<DanhMuc> DanhMucCons { get; set; } = new List<DanhMuc>();
    }
}
