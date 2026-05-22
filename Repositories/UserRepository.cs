using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OnlineMedicineStoreDbContext _context;

        public UserRepository(OnlineMedicineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NguoiDung>> GetAllUsersAsync()
        {
            // Sắp xếp người mới tạo lên đầu
            return await _context.NguoiDungs.OrderByDescending(u => u.NgayTao).ToListAsync();
        }

        public async Task<NguoiDung?> GetUserByIdAsync(Guid id)
        {
            return await _context.NguoiDungs.FindAsync(id);
        }

        public async Task<NguoiDung> AddUserAsync(NguoiDung user)
        {
            _context.NguoiDungs.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<NguoiDung> UpdateUserAsync(NguoiDung user)
        {
            _context.NguoiDungs.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.NguoiDungs.FindAsync(id);
            if (user == null) return false;

            _context.NguoiDungs.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}