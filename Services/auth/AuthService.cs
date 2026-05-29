using Google.Apis.Auth;
using OnlineMedicineStoreBackend.DTOs.auth;
using OnlineMedicineStoreBackend.Models;
using OnlineMedicineStoreBackend.Repositories;
using OnlineMedicineStoreBackend.utils;
using Microsoft.Extensions.Caching.Memory;

namespace OnlineMedicineStoreBackend.Services.auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserDRepository _userRepo;
        private readonly JwtUtils _jwtUtils;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;

        public AuthService(IUserDRepository userRepo, JwtUtils jwtUtils, IMemoryCache cache, IEmailService emailService)
        {
        _userRepo = userRepo;
        _jwtUtils = jwtUtils;
        _cache = cache;
        _emailService = emailService;
        }

        public async Task<ApiResponse<string>> LoginWithGoogleAsync(string googleToken)
        {
            if (string.IsNullOrWhiteSpace(googleToken))
            {
                return new ApiResponse<string> { Success = false, Message = "Token Google không được để trống." };
            }

            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken);
                
                if (payload == null || string.IsNullOrWhiteSpace(payload.Email))
                {
                    return new ApiResponse<string> { Success = false, Message = "Thông tin xác thực từ Google không hợp lệ." };
                }

                var user = await _userRepo.GetUserByEmailAsync(payload.Email);

                // Nếu chưa có tài khoản, tự động tạo mới
                if (user == null)
                {
                    
                    string emailPrefix = payload.Email.Split('@')[0];
                    string username = emailPrefix;
                    
                    bool isExist = await _userRepo.IsUsernameExistAsync(username);
                    if (isExist) 
                    {
                        username = $"{emailPrefix}_{Guid.NewGuid().ToString("N")[..4]}";
                    }

                    user = new NguoiDung
                    {
                        TenDangNhap = username,
                        MatKhau = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                        HoTen = payload.Name ?? "Người dùng Google",
                        Email = payload.Email,
                        GioiTinh = null,
                        VaiTro = "KhachHang",
                        NgayTao = DateTime.UtcNow,
                        TrangThai = true
                    };

                    var customer = new KhachHang
                    {
                        DiemTichLuy = 0,
                        HangThanhVien = "Đồng"
                    };

                    await _userRepo.CreateUserWithCustomerProfileAsync(user, customer);
                }

                // Tạo JWT Token hệ thống
                var systemToken = _jwtUtils.GenerateJwtToken(user);
                
                return new ApiResponse<string> 
                { 
                    Success = true, 
                    Message = "Đăng nhập thành công.", 
                    Data = systemToken,
                    Role = user.VaiTro
                };
            }
            catch (InvalidJwtException)
            {
                return new ApiResponse<string> { Success = false, Message = "Chữ ký Token Google không hợp lệ hoặc đã hết hạn." };
            }
            catch (Exception ex)
            {
                // Bắt lỗi Database hoặc lỗi không lường trước
                return new ApiResponse<string> { Success = false, Message = $"Lỗi xử lý: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterDto dto)
        {
            if (dto == null) return new ApiResponse<string> { Success = false, Message = "Dữ liệu trống." };

            //Validate Email & Username đã tồn tại chưa
            if (await _userRepo.GetUserByEmailAsync(dto.Email) != null)
                return new ApiResponse<string> { Success = false, Message = "Email này đã được đăng ký." };
            
            if (await _userRepo.IsUsernameExistAsync(dto.TenDangNhap))
                return new ApiResponse<string> { Success = false, Message = "Tên đăng nhập đã tồn tại." };

            // Tạo OTP 6 số
            var otp = new Random().Next(100000, 999999).ToString();

            // 3. Lưu thông tin tạm vào RAM (Cache) trong đúng 60 giây
            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
            _cache.Set($"Register_{dto.Email}", dto, cacheOptions);
            _cache.Set($"OTP_{dto.Email}", otp, cacheOptions);

            // Gửi Email
            await _emailService.SendEmailAsync(dto.Email, "Mã xác thực đăng ký", $"Mã OTP của bạn là: {otp}. Mã có hiệu lực trong 60 giây.");

            return new ApiResponse<string> { Success = true, Message = "Mã xác thực đã được gửi đến email." };
        }

        public async Task<ApiResponse<string>> VerifyOtpAsync(VerifyOtpDto dto)
        {
            //  Lấy OTP từ Cache
            if (!_cache.TryGetValue($"OTP_{dto.Email}", out string? savedOtp) || savedOtp != dto.Otp)
            {
                // Kiểm tra xem là nhập sai hay do hết hạn
                if (!_cache.TryGetValue($"Register_{dto.Email}", out _)) 
                    return new ApiResponse<string> { Success = false, Message = "Mã xác thực đã hết hạn." };
                
                return new ApiResponse<string> { Success = false, Message = "Mã xác thực không hợp lệ." };
            }

            //  Nếu OTP đúng, lấy thông tin RegisterDto từ Cache ra để tạo User
            var registerData = _cache.Get<RegisterDto>($"Register_{dto.Email}");
            if (registerData == null) return new ApiResponse<string> { Success = false, Message = "Phiên đăng ký đã hết hạn." };

            var user = new NguoiDung
            {
                HoTen = registerData.HoTen,
                TenDangNhap = registerData.TenDangNhap,
                SoDienThoai = registerData.SoDienThoai,
                Email = registerData.Email,
                MatKhau = BCrypt.Net.BCrypt.HashPassword(registerData.MatKhau), // Mã hóa mật khẩu
                VaiTro = "KhachHang",
                NgayTao = DateTime.UtcNow,
                TrangThai = true
            };

            var customer = new KhachHang { DiemTichLuy = 0, HangThanhVien = "Đồng" };

            await _userRepo.CreateUserWithCustomerProfileAsync(user, customer);
            
            _cache.Remove($"OTP_{dto.Email}");
            _cache.Remove($"Register_{dto.Email}");

            return new ApiResponse<string> { Success = true, Message = "Đăng ký thành công!" };
        }

        public async Task<ApiResponse<string>> LoginAsync(LoginDto dto)
        {
            
            var user = await _userRepo.GetUserByUsernameAsync(dto.TenDangNhap);
            
            
            if (user == null)
            {
                return new ApiResponse<string> { Success = false, Message = "Tên đăng nhập hoặc mật khẩu không chính xác." };
            }

            
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.MatKhau, user.MatKhau);
            if (!isPasswordValid)
            {
                return new ApiResponse<string> { Success = false, Message = "Tên đăng nhập hoặc mật khẩu không chính xác." };
            }

            
            var token = _jwtUtils.GenerateJwtToken(user);

            return new ApiResponse<string> 
            { 
                Success = true, 
                Message = "Đăng nhập thành công.", 
                Data = token, 
                Role = user.VaiTro 
            };
        }

        

        
    }
}