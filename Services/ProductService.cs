using OnlineMedicineStoreBackend.Models;
using OnlineMedicineStoreBackend.Repositories;

namespace OnlineMedicineStoreBackend.Services
{
    public class ProductService : IProductService
    {
        private readonly IThuocRepository _thuocRepository;

        public ProductService(IThuocRepository thuocRepository)
        {
            _thuocRepository = thuocRepository;
        }

        public async Task<IEnumerable<Thuoc>> GetDanhSachThuocAsync(string? tuKhoa, Guid? maDanhMuc)
        {
            var tatCaThuoc = await _thuocRepository.GetAllAsync();
            var query = tatCaThuoc.AsEnumerable();

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query = query.Where(t => t.TenThuoc.ToLower().Contains(tuKhoa.ToLower()));
            }
            if (maDanhMuc.HasValue)
            {
                query = query.Where(t => t.MaDanhMuc == maDanhMuc.Value);
            }

            return query.ToList();
        }

        public async Task<Thuoc?> GetChiTietThuocAsync(Guid id)
        {
            return await _thuocRepository.GetByIdAsync(id);
        }

        // --- HÀM THÊM SẢN PHẨM ---
        public async Task<Thuoc> AddThuocAsync(Thuoc thuoc)
        {
            thuoc.MaThuoc = Guid.NewGuid(); // Tự động tạo mã ID mới không bị trùng
            return await _thuocRepository.AddAsync(thuoc);
        }

        // --- HÀM XÓA SẢN PHẨM ---
        public async Task<bool> DeleteThuocAsync(Guid id)
        {
            return await _thuocRepository.DeleteAsync(id);
        }
    }
}