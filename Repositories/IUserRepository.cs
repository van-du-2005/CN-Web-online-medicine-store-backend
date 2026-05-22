using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<NguoiDung>> GetAllUsersAsync();
        Task<NguoiDung?> GetUserByIdAsync(Guid id);
        Task<NguoiDung> AddUserAsync(NguoiDung user);
        Task<NguoiDung> UpdateUserAsync(NguoiDung user);
        Task<bool> DeleteUserAsync(Guid id);
    }
}