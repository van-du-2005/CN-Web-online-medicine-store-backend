using Microsoft.EntityFrameworkCore.Storage;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;
namespace OnlineMedicineStoreBackend.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineMedicineStoreCNWDbContext _dbContext;
        private IDbContextTransaction? _dbContextTransaction;
        public IGenericRepository<Thuoc> Thuocs { get; private set; }
        public IGenericRepository<DiaChiKhachHang> DiaChiKhachHangs { get; private set; }
        public IGenericRepository<ChiTietDonHang> ChiTietDonHangs { get; private set; }
        public IGenericRepository<LoThuoc> LoThuocs { get; private set; }
        // Có thêm các hàm phụ nên không thể dùng GenericRepository cho DonHang được, nên phải tạo riêng 1 repository cho DonHang
        public IDonHangRepository DonHangs { get; private set; }
        public IGenericRepository<KhachHang> KhachHangs { get; private set; }

        public UnitOfWork(OnlineMedicineStoreCNWDbContext dbContext)
        {
            _dbContext = dbContext;
            Thuocs = new GenericRepository<Thuoc>(_dbContext);
            DiaChiKhachHangs = new GenericRepository<DiaChiKhachHang>(_dbContext);
            ChiTietDonHangs = new GenericRepository<ChiTietDonHang>(_dbContext);
            LoThuocs = new GenericRepository<LoThuoc>(_dbContext);
            KhachHangs = new GenericRepository<KhachHang>(_dbContext);

            DonHangs = new DonHangRepository(_dbContext);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        // Các phương thức quản lý giao dịch TRANSACTION
        public async Task BeginTransactionAsync()
        {
            if(_dbContextTransaction != null)
            {
                throw new InvalidOperationException("Một Transaction khác đang được thực thi.");
            }
            _dbContextTransaction = await _dbContext.Database.BeginTransactionAsync();
        }
        
        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                if (_dbContextTransaction != null)
                {
                    await _dbContextTransaction.CommitAsync();
                }
            }
            finally
            {
                if(_dbContextTransaction != null)
                {
                    await _dbContextTransaction.DisposeAsync();
                    _dbContextTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_dbContextTransaction != null)
                {
                    await _dbContextTransaction.RollbackAsync();
                }
            }
            finally
            {
                if (_dbContextTransaction != null)
                {
                    await _dbContextTransaction.DisposeAsync();
                    _dbContextTransaction = null;
                }
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this); // Ngăn trình thu gom rác gọi đến finalizer của đối tượng này
        }
    }
}