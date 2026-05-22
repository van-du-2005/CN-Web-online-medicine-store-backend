using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories
{
    public interface IUserDRepository
    {
        Task<NguoiDung?> GetUserByEmailAsync(string email);
        Task<bool> IsUsernameExistAsync(string username);
        Task CreateUserWithCustomerProfileAsync(NguoiDung user, KhachHang customer);
        Task<NguoiDung?> GetUserByUsernameAsync(string username);
    }
}