using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Thuoc>> GetDanhSachThuocAsync(string? tuKhoa, Guid? maDanhMuc);
        Task<Thuoc?> GetChiTietThuocAsync(Guid id);

        // --- 2 HÀM MỚI CHO ADMIN ---
        Task<Thuoc> AddThuocAsync(Thuoc thuoc);
        Task<bool> DeleteThuocAsync(Guid id);
    }
}