using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<NguoiDung>> GetAllUsersAsync();
        Task<NguoiDung?> GetUserByIdAsync(Guid id);
        Task<bool> DeleteUserAsync(Guid id);
    }
}