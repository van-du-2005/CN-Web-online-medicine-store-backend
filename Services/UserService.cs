using OnlineMedicineStoreBackend.DTOs;
using OnlineMedicineStoreBackend.Models;
using OnlineMedicineStoreBackend.Repositories;

namespace OnlineMedicineStoreBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Hàm dùng chung để "Dịch" từ Database (C#) sang giao diện (Angular)
        private NguoiDungDto MapToDto(NguoiDung user)
        {
            return new NguoiDungDto
            {
                Id = user.MaNguoiDung,
                HoTen = user.HoTen,
                Avatar = "https://placehold.co/150x150?text=" + (string.IsNullOrEmpty(user.HoTen) ? "U" : user.HoTen.Substring(0, 1).ToUpper()),
                Email = user.Email,
                Sdt = user.SoDienThoai,
                VaiTro = user.VaiTro,
                TrangThai = user.TrangThai
            };
        }

        public async Task<IEnumerable<NguoiDungDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(MapToDto); // Dịch toàn bộ danh sách sang DTO
        }

        public async Task<NguoiDungDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<NguoiDungDto> AddUserAsync(TaoNguoiDungDto dto)
        {
            // Cắt phần đầu của email làm Tên đăng nhập mặc định (hoặc tạo random)
            string usernameMoi = (dto.Email ?? "user").Split('@')[0] + new Random().Next(100, 999);

            var newUser = new NguoiDung
            {
                MaNguoiDung = Guid.NewGuid(),
                TenDangNhap = usernameMoi,
                HoTen = dto.HoTen,
                Email = dto.Email,
                SoDienThoai = dto.Sdt,
                VaiTro = dto.VaiTro,
                MatKhau = BCrypt.Net.BCrypt.HashPassword(dto.MatKhau), // Mã hóa mật khẩu bảo mật
                NgayTao = DateTime.Now,
                TrangThai = true
            };

            var savedUser = await _userRepository.AddUserAsync(newUser);
            return MapToDto(savedUser);
        }

        public async Task<NguoiDungDto?> UpdateUserAsync(Guid id, CapNhatNguoiDungDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            user.HoTen = dto.HoTen;
            user.Email = dto.Email;
            user.SoDienThoai = dto.Sdt;
            user.VaiTro = dto.VaiTro;

            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return MapToDto(updatedUser);
        }

        public async Task<bool> ToggleStatusAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return false;

            user.TrangThai = !user.TrangThai; // Đảo ngược trạng thái hiện tại
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }
    }
}