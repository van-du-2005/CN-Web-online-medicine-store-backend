using OnlineMedicineStoreBackend.Models;
namespace OnlineMedicineStoreBackend.Services
{
    public interface IZaloPayService
    {
        Task<string> CreatePaymentAsync(DonHang donHang);
        (bool IsSuccess, Guid MaDonHang) ProcessZaloPayReturn(int status, string transactionId); // Tuple là kiểu dữ liệu trả về nhiều giá trị, ở đây là IsSuccess và MaDonHang
    }
}