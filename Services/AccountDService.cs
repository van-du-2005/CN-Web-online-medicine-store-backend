using OnlineMedicineStoreBackend.DTOs.account;
using OnlineMedicineStoreBackend.DTOs.auth;
using OnlineMedicineStoreBackend.Models;
using OnlineMedicineStoreBackend.Repositories;

namespace OnlineMedicineStoreBackend.Services
{
    public class AccountDService : IAccountDService
    {
        private readonly IAccountDRepository _repo;

        public AccountDService(IAccountDRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<UserProfileDto>> GetProfileAsync(Guid userId)
        {
            var user = await _repo.GetUserWithCustomerInfoAsync(userId);
            if (user == null) return new ApiResponse<UserProfileDto> { Success = false, Message = "Không tìm thấy người dùng." };

            var dto = new UserProfileDto
            {
                HoTen = user.HoTen,
                SoDienThoai = user.SoDienThoai,
                Email = user.Email,
                NgaySinh = user.NgaySinh,
                GioiTinh = user.GioiTinh,
                DiaChi = user.DiaChi,
                HangThanhVien = user.KhachHang?.HangThanhVien ?? "Đồng",
                DiemTichLuy = user.KhachHang?.DiemTichLuy ?? 0
            };

            return new ApiResponse<UserProfileDto> { Success = true, Message = "Lấy thông tin thành công.", Data = dto };
        }

        public async Task<ApiResponse<string>> UpdateProfileAsync(Guid userId, UserProfileDto dto)
        {
            var user = await _repo.GetUserWithCustomerInfoAsync(userId);
            if (user == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy người dùng." };

            user.HoTen = dto.HoTen;
            user.SoDienThoai = dto.SoDienThoai;
            user.NgaySinh = dto.NgaySinh;
            user.GioiTinh = dto.GioiTinh;
            user.DiaChi = dto.DiaChi;

            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Cập nhật thông tin thành công." };
        }

        public async Task<ApiResponse<string>> ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
        {
            var user = await _repo.GetUserWithCustomerInfoAsync(userId);
            if (user == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy người dùng." };

            if (!BCrypt.Net.BCrypt.Verify(dto.MatKhauCu, user.MatKhau))
                return new ApiResponse<string> { Success = false, Message = "Mật khẩu hiện tại không chính xác." };

            user.MatKhau = BCrypt.Net.BCrypt.HashPassword(dto.MatKhauMoi);
            await _repo.SaveChangesAsync();

            return new ApiResponse<string> { Success = true, Message = "Đổi mật khẩu thành công." };
        }

        public async Task<ApiResponse<List<OrderHistoryDto>>> GetOrdersAsync(Guid userId, string filter)
        {
            var customer = await _repo.GetCustomerByUserIdAsync(userId);
            if (customer == null) return new ApiResponse<List<OrderHistoryDto>> { Success = false, Message = "Tài khoản không hợp lệ." };

            var orders = await _repo.GetOrdersByCustomerIdAsync(customer.MaKhachHang, filter);
            
            var result = orders.Select(o => new OrderHistoryDto
            {
                MaDonHang = o.MaDonHang,
                NgayDat = o.NgayDat,
                TrangThai = o.TrangThai,
                TamTinh = o.TamTinh,
                PhiVanChuyen = o.PhiVanChuyen,
                GiamGia = o.GiamGia,
                ThanhToan = o.ThanhToan,
                ChiTiet = o.ChiTietDonHangs.Select(ct => new OrderDetailDto
                {
                    TenThuoc = ct.Thuoc?.TenThuoc ?? "Sản phẩm không xác định",
                    HinhAnh = !string.IsNullOrEmpty(ct.Thuoc?.HinhAnh) 
                                ? (ct.Thuoc.HinhAnh.StartsWith("/") ? ct.Thuoc.HinhAnh : "/" + ct.Thuoc.HinhAnh) 
                                : "/anh_mac_dinh.png", 
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia
                }).ToList()
            }).ToList();

            return new ApiResponse<List<OrderHistoryDto>> { Success = true, Message = "Thành công.", Data = result };
        }

        public async Task<ApiResponse<string>> CancelOrderAsync(Guid userId, Guid orderId)
        {
            var customer = await _repo.GetCustomerByUserIdAsync(userId);
            if (customer == null) return new ApiResponse<string> { Success = false, Message = "Lỗi xác thực." };

            var order = await _repo.GetOrderByIdAsync(orderId, customer.MaKhachHang);
            if (order == null || order.TrangThai != "ChoXacNhan")
                return new ApiResponse<string> { Success = false, Message = "Không thể hủy đơn hàng này do đã được xử lý." };

            order.TrangThai = "DaHuy";
            await _repo.SaveChangesAsync();

            return new ApiResponse<string> { Success = true, Message = "Hủy đơn hàng thành công." };
        }

        public async Task<ApiResponse<List<AddressDto>>> GetAddressesAsync(Guid userId)
        {
            var customer = await _repo.GetCustomerByUserIdAsync(userId);
            if (customer == null) return new ApiResponse<List<AddressDto>> { Success = false, Message = "Lỗi xác thực." };

            var addresses = await _repo.GetAddressesAsync(customer.MaKhachHang);
            var result = addresses.Select(a => new AddressDto
            {
                MaDiaChi = a.MaDiaChi,
                HoTenNguoiNhan = a.HoTenNguoiNhan,
                SdtNguoiNhan = a.SDTNguoiNhan,
                DiaChi = a.DiaChi,
                LaMacDinh = a.LaMacDinh
            }).ToList();

            return new ApiResponse<List<AddressDto>> { Success = true, Message = "Thành công.", Data = result };
        }

        public async Task<ApiResponse<string>> AddAddressAsync(Guid userId, AddressDto dto)
        {
            var customer = await _repo.GetCustomerByUserIdAsync(userId);
            if (customer == null) return new ApiResponse<string> { Success = false, Message = "Lỗi xác thực." };

            if (dto.LaMacDinh)
            {
                var existing = await _repo.GetAddressesAsync(customer.MaKhachHang);
                foreach (var item in existing.Where(x => x.LaMacDinh)) item.LaMacDinh = false;
            }

            var newAddress = new DiaChiKhachHang
            {
                MaKhachHang = customer.MaKhachHang,
                HoTenNguoiNhan = dto.HoTenNguoiNhan,
                SDTNguoiNhan = dto.SdtNguoiNhan,
                DiaChi = dto.DiaChi,
                LaMacDinh = dto.LaMacDinh
            };

            await _repo.AddAddressAsync(newAddress);
            await _repo.SaveChangesAsync();

            return new ApiResponse<string> { Success = true, Message = "Thêm địa chỉ thành công." };
        }

        public async Task<ApiResponse<string>> DeleteAddressAsync(Guid userId, Guid addressId)
        {
            var customer = await _repo.GetCustomerByUserIdAsync(userId);
            if (customer == null) return new ApiResponse<string> { Success = false, Message = "Lỗi xác thực." };

            var address = await _repo.GetAddressByIdAsync(addressId, customer.MaKhachHang);
            if (address == null) return new ApiResponse<string> { Success = false, Message = "Địa chỉ không tồn tại." };

            await _repo.DeleteAddressAsync(address);
            await _repo.SaveChangesAsync();

            return new ApiResponse<string> { Success = true, Message = "Xóa địa chỉ thành công." };
        }

        public async Task<ApiResponse<string>> SetDefaultAddressAsync(Guid userId, Guid addressId)
        {
            var customer = await _repo.GetCustomerByUserIdAsync(userId);
            if (customer == null) return new ApiResponse<string> { Success = false, Message = "Lỗi xác thực." };

            var allAddresses = await _repo.GetAddressesAsync(customer.MaKhachHang);
            foreach (var addr in allAddresses)
            {
                addr.LaMacDinh = (addr.MaDiaChi == addressId);
            }

            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Đã đặt làm địa chỉ mặc định." };
        }
    }
}