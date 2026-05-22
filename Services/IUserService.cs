using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Services
{
    public interface IUserService
    {
        Task<IEnumerable<NguoiDung>> GetAllUsersAsync();
        Task<NguoiDung?> GetUserByIdAsync(Guid id);
        Task<bool> DeleteUserAsync(Guid id);
    }
}