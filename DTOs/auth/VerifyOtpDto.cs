using System.ComponentModel.DataAnnotations;

namespace OnlineMedicineStoreBackend.DTOs.auth
{
    public class VerifyOtpDto
    {
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã OTP không được để trống")]
        public string Otp { get; set; } = string.Empty;
    }
}