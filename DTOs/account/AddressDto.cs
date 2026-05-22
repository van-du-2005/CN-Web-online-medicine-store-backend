using System.ComponentModel.DataAnnotations;

namespace OnlineMedicineStoreBackend.DTOs.account
{
    public class AddressDto
    {
        public Guid? MaDiaChi { get; set; } 
        
        [Required(ErrorMessage = "Vui lòng nhập họ tên người nhận")]
        public string HoTenNguoiNhan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SdtNguoiNhan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string DiaChi { get; set; } = string.Empty;
        public bool LaMacDinh { get; set; }
    }
}