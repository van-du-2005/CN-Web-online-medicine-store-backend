using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories
{
    public class UserDRepository : IUserDRepository
    {
        private readonly OnlineMedicineStoreCNWDbContext _context;

        public UserDRepository(OnlineMedicineStoreCNWDbContext context)
        {
            _context = context;
        }

        public async Task<NguoiDung?> GetUserByEmailAsync(string email)
        {
            return await _context.NguoiDungs
                .FirstOrDefaultAsync(u => u.Email == email && u.TrangThai == true);
        }

        public async Task<bool> IsUsernameExistAsync(string username)
        {
            return await _context.NguoiDungs.AnyAsync(u => u.TenDangNhap == username);
        }

        public async Task CreateUserWithCustomerProfileAsync(NguoiDung user, KhachHang customer)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.NguoiDungs.AddAsync(user);
                await _context.SaveChangesAsync(); 

                customer.MaNguoiDung = user.MaNguoiDung;
                await _context.KhachHangs.AddAsync(customer);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<NguoiDung?> GetUserByUsernameAsync(string username)
        {
            return await _context.NguoiDungs
                .FirstOrDefaultAsync(u => u.TenDangNhap == username && u.TrangThai == true);
        }
    }
}