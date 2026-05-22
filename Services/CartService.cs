using OnlineMedicineStoreBackend.DTOs;
using OnlineMedicineStoreBackend.Repositories;
using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Services
{
    public class CartService : ICartService
    {
        private readonly IGioHangRepository _gioHangRepository;
        private readonly IGenericRepository<Thuoc> _thuocRepository;

        public CartService(IGioHangRepository gioHangRepository, IGenericRepository<Thuoc> thuocRepository)
        {
            _gioHangRepository = gioHangRepository;
            _thuocRepository = thuocRepository;
        }

        public async Task<CartDto> GetCartAsync(Guid userId)
        {
            // ===== TEST DATA - Luôn trả về dữ liệu test (Xóa khi deploy production) =====
            return new CartDto
            {
                Items = new List<CartItemDto>
                {
                    new CartItemDto
                    {
                        MaThuoc = Guid.Parse("550e8400-e29b-41d4-a716-446655440001"),
                        TenThuoc = "Paracetamol 500mg",
                        HinhAnh = "https://via.placeholder.com/200?text=Paracetamol",
                        SoLuong = 2,
                        Gia = 50000
                    },
                    new CartItemDto
                    {
                        MaThuoc = Guid.Parse("550e8400-e29b-41d4-a716-446655440002"),
                        TenThuoc = "Ibuprofen 400mg",
                        HinhAnh = "https://via.placeholder.com/200?text=Ibuprofen",
                        SoLuong = 1,
                        Gia = 75000
                    },
                    new CartItemDto
                    {
                        MaThuoc = Guid.Parse("550e8400-e29b-41d4-a716-446655440003"),
                        TenThuoc = "Vitamin C 1000mg",
                        HinhAnh = "https://via.placeholder.com/200?text=VitaminC",
                        SoLuong = 3,
                        Gia = 35000
                    }
                }
            };
            // ===== KẾT THÚC TEST DATA =====

            /*
            // Code thực tế (tạm comment)

            var cart = await _gioHangRepository.GetCartWithDetailsAsync(userId);
            var cartDto = new CartDto
            {
                Items = new List<CartItemDto>()
            };

            if (cart == null || cart.ChiTietGioHangs == null || !cart.ChiTietGioHangs.Any())
            {
                return cartDto;
            }

            // Dùng list tạm để chứa các tác vụ cần đồng bộ xuống DB, tránh gọi DB liên tục trong vòng lặp
            var itemsToRemove = new List<Guid>();
            var itemsToUpdate = new List<(Guid MaThuoc, int SoLuongMoi)>();

            foreach (var item in cart.ChiTietGioHangs)
            {
                if (item.Thuoc == null) continue;

                // 1. Nếu thuốc đã ngừng bán hoặc hết hàng -> Đánh dấu để xóa
                if (!item.Thuoc.TrangThai || item.Thuoc.TonKhoHienTai == 0)
                {
                    itemsToRemove.Add(item.MaThuoc);
                    continue; 
                }

                // 2. Cân bằng số lượng giỏ hàng và tồn kho
                int soLuongThucTe = item.SoLuong;
                if (item.SoLuong > item.Thuoc.TonKhoHienTai)
                {
                    soLuongThucTe = item.Thuoc.TonKhoHienTai;
                    itemsToUpdate.Add((item.MaThuoc, soLuongThucTe)); // Đánh dấu để cập nhật DB
                }

                cartDto.Items.Add(new CartItemDto
                {
                    MaThuoc = item.MaThuoc,
                    TenThuoc = item.Thuoc.TenThuoc,
                    HinhAnh = item.Thuoc.HinhAnh,
                    SoLuong = soLuongThucTe,
                    Gia = item.Thuoc.GiaBan
                });
            }

            // 3. Thực thi đồng bộ hóa database sau khi đã duyệt xong
            foreach (var maThuoc in itemsToRemove)
            {
                await _gioHangRepository.RemoveCartItemAsync(userId, maThuoc);
            }

            foreach (var update in itemsToUpdate)
            {
                await _gioHangRepository.UpdateCartItemQuantityAsync(userId, update.MaThuoc, update.SoLuongMoi);
            }

            return cartDto;
            */
        }

        public async Task<bool> AddCartAsync(Guid userId, Guid maThuoc, int soLuong)
        {
            if (soLuong <= 0) return false;

            var thuoc = await _thuocRepository.GetByIdAsync(maThuoc);

            if (thuoc == null || !thuoc.TrangThai || thuoc.TonKhoHienTai < soLuong)
            {
                return false; 
            }

            // Lưu ý: Đảm bảo _gioHangRepository.AddCartItemAsync đã có logic "Cộng dồn" 
            // nếu thuốc đó đã tồn tại sẵn trong giỏ hàng.
            await _gioHangRepository.AddCartItemAsync(userId, maThuoc, soLuong);

            return true;
        }

        public async Task<bool> UpdateCartItemQuantityAsync(Guid userId, Guid maThuoc, int soLuong)
        {
            if (soLuong <= 0) return false;

            var thuoc = await _thuocRepository.GetByIdAsync(maThuoc);

            if (thuoc == null || !thuoc.TrangThai || thuoc.TonKhoHienTai < soLuong)
            {
                return false; 
            }

            await _gioHangRepository.UpdateCartItemQuantityAsync(userId, maThuoc, soLuong);

            return true;
        }

        public async Task<bool> RemoveCartItemAsync(Guid userId, Guid maThuoc)
        {
            await _gioHangRepository.RemoveCartItemAsync(userId, maThuoc);
            return true;
        }
    }
}