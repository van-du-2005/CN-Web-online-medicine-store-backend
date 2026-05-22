using OnlineMedicineStoreBackend.DTOs.auth;

namespace OnlineMedicineStoreBackend.Services.auth
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> LoginWithGoogleAsync(string googleToken);
        Task<ApiResponse<string>> RegisterAsync(RegisterDto dto);
        Task<ApiResponse<string>> VerifyOtpAsync(VerifyOtpDto dto);
        Task<ApiResponse<string>> LoginAsync(LoginDto dto);
    }
}