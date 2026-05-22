using System.ComponentModel.DataAnnotations;

namespace OnlineMedicineStoreBackend.DTOs.account
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại")]
        public string MatKhauCu { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [MinLength(6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự")]
        public string MatKhauMoi { get; set; } = string.Empty;
    }
}