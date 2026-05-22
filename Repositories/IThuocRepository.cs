using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories
{
    public interface IThuocRepository
    {
        Task<IEnumerable<Thuoc>> GetAllAsync();
        Task<Thuoc?> GetByIdAsync(Guid id);

        // --- 2 HÀM MỚI CHO ADMIN ---
        Task<Thuoc> AddAsync(Thuoc thuoc);
        Task<bool> DeleteAsync(Guid id);
    }
}