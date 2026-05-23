using OnlineMedicineStoreBackend.Models;
using OnlineMedicineStoreBackend.DTOs;

namespace OnlineMedicineStoreBackend.Services
{
    public interface IOrderService
    {
        Task<DonHang> CreateOrderAsync(CheckoutDTO checkoutDTO);
        Task<bool> UpdatePaymentStatusAsync(Guid maDonHang, string trangThaiDonHang);
    }
}