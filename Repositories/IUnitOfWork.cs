using OnlineMedicineStoreBackend.Models;
namespace OnlineMedicineStoreBackend.Repositories
{
    public interface IUnitOfWork: IDisposable
    {
        IGenericRepository<Thuoc> Thuocs{ get; }
        IDonHangRepository DonHangs { get; }
        IGenericRepository<DiaChiKhachHang> DiaChiKhachHangs { get; }
        IGenericRepository<ChiTietDonHang> ChiTietDonHangs { get; }
        IGenericRepository<LoThuoc> LoThuocs { get; }
        public IGenericRepository<KhachHang> KhachHangs { get; }

        Task<int> SaveChangesAsync();

        // Các phương thức quản lý giao dịch TRANSACTION
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}