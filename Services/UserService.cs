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

        public async Task<IEnumerable<NguoiDung>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<NguoiDung?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }
    }
}