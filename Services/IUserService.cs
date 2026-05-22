using OnlineMedicineStoreBackend.DTOs;

namespace OnlineMedicineStoreBackend.Services
{
    public interface IUserService
    {
        Task<IEnumerable<NguoiDungDto>> GetAllUsersAsync();
        Task<NguoiDungDto?> GetUserByIdAsync(Guid id);

        // --- CÁC HÀM MỚI BỔ SUNG CHO ADMIN ---
        Task<NguoiDungDto> AddUserAsync(TaoNguoiDungDto dto);
        Task<NguoiDungDto?> UpdateUserAsync(Guid id, CapNhatNguoiDungDto dto);
        Task<bool> ToggleStatusAsync(Guid id); // Khóa / Mở khóa

        Task<bool> DeleteUserAsync(Guid id);
    }
}