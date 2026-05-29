using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories
{
    public interface IAccountDRepository
    {
        Task<NguoiDung?> GetUserWithCustomerInfoAsync(Guid userId);
        Task<KhachHang?> GetCustomerByUserIdAsync(Guid userId);
        Task<List<DonHang>> GetOrdersByCustomerIdAsync(Guid customerId, string filter);
        Task<DonHang?> GetOrderByIdAsync(Guid orderId, Guid customerId);
        Task<List<DiaChiKhachHang>> GetAddressesAsync(Guid customerId);
        Task<DiaChiKhachHang?> GetAddressByIdAsync(Guid addressId, Guid customerId);
        Task AddAddressAsync(DiaChiKhachHang address);
        Task DeleteAddressAsync(DiaChiKhachHang address);
        Task SaveChangesAsync();
    }
}