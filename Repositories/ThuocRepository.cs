using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories
{
    public class ThuocRepository : IThuocRepository
    {
        private readonly OnlineMedicineStoreDbContext _context;

        public ThuocRepository(OnlineMedicineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Thuoc>> GetAllAsync()
        {
            return await _context.Thuocs.ToListAsync();
        }

        public async Task<Thuoc?> GetByIdAsync(Guid id)
        {
            return await _context.Thuocs.FindAsync(id);
        }

        // --- HÀM THÊM SẢN PHẨM ---
        public async Task<Thuoc> AddAsync(Thuoc thuoc)
        {
            _context.Thuocs.Add(thuoc);
            await _context.SaveChangesAsync(); // Lưu thay đổi vào DB
            return thuoc;
        }

        // --- HÀM XÓA SẢN PHẨM ---
        public async Task<bool> DeleteAsync(Guid id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return false;

            _context.Thuocs.Remove(thuoc);
            await _context.SaveChangesAsync(); // Lưu thay đổi vào DB
            return true;
        }
    }
}