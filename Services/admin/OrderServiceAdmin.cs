using OnlineMedicineStoreBackend.DTOs.admin;
using OnlineMedicineStoreBackend.DTOs.auth;
using OnlineMedicineStoreBackend.Repositories.admin;

namespace OnlineMedicineStoreBackend.Services.admin
{
    public class OrderServiceAdmin : IOrderServiceAdmin
    {
        private readonly IOrderRepositoryAdmin _repo;
        private const string DEFAULT_IMAGE = "/anh_mac_dinh.png"; 

        public OrderServiceAdmin(IOrderRepositoryAdmin repo) => _repo = repo;

        public async Task<ApiResponse<OrderFilterCountDto>> GetFilterCountsAsync()
        {
            var dict = await _repo.GetOrderCountsAsync();
            var data = new OrderFilterCountDto
            {
                TatCa = dict.Values.Sum(),
                ChoXacNhan = dict.GetValueOrDefault("ChoXacNhan", 0),
                DangXuLy = dict.GetValueOrDefault("DangXuLy", 0),
                DangGiao = dict.GetValueOrDefault("DangGiao", 0),
                DaGiao = dict.GetValueOrDefault("DaGiao", 0),
                DaHuy = dict.GetValueOrDefault("DaHuy", 0)
            };
            return new ApiResponse<OrderFilterCountDto> { Success = true, Message = "Thành công", Data = data };
        }

        public async Task<ApiResponse<PagedOrderResultDto>> GetOrdersAsync(string status, int page, int limit)
        {
            if (page < 1) page = 1;
            if (limit <= 0 || limit > 50) limit = 15;

            var (items, totalCount) = await _repo.GetPagedOrdersAsync(status, page, limit);
            int totalPages = (int)Math.Ceiling(totalCount / (double)limit);

            var vmItems = items.Select(d => new OrderListItemDto
            {
                MaDonHang = d.MaDonHang,
                TenKhachHang = d.KhachHang?.NguoiDung?.HoTen ?? "Khách vãng lai",
                NgayDatFormat = d.NgayDat.ToString("dd/MM/yyyy - HH:mm"),
                TongTien = d.ThanhToan,
                TrangThai = d.TrangThai,
                TrangThaiLabel = GetStatusLabel(d.TrangThai),
                ColorCode = GetStatusColor(d.TrangThai)
            }).ToList();

            var result = new PagedOrderResultDto
            {
                Items = vmItems,
                CurrentPage = page,
                TotalPages = totalPages == 0 ? 1 : totalPages,
                TotalItems = totalCount
            };

            return new ApiResponse<PagedOrderResultDto> { Success = true, Message = "Thành công", Data = result };
        }

        public async Task<ApiResponse<OrderDetailDto>> GetOrderDetailAsync(Guid maDonHang)
        {
            var d = await _repo.GetOrderDetailAsync(maDonHang);
            if (d == null) return new ApiResponse<OrderDetailDto> { Success = false, Message = "Không tìm thấy đơn hàng." };

            var dto = new OrderDetailDto
            {
                MaDonHang = d.MaDonHang,
                NgayDatFormat = d.NgayDat.ToString("dd/MM/yyyy - HH:mm"),
                HoTen = d.DiaChiKhachHang?.HoTenNguoiNhan ?? d.KhachHang?.NguoiDung?.HoTen ?? "Không có thông tin",
                SoDienThoai = d.DiaChiKhachHang?.SDTNguoiNhan ?? d.KhachHang?.NguoiDung?.SoDienThoai ?? "Không có thông tin",
                DiaChi = d.DiaChiKhachHang?.DiaChi ?? "Mua tại quầy",
                TrangThai = d.TrangThai,
                PhuongThucThanhToan = string.IsNullOrEmpty(d.PhuongThucThanhToan) ? "Thanh toán khi nhận hàng (COD)" : d.PhuongThucThanhToan,
                PhiVanChuyen = d.PhiVanChuyen,
                TamTinh = d.TamTinh,
                GiamGia = d.GiamGia,
                TongTien = d.ThanhToan
            };

            int stt = 1;
            foreach (var ct in d.ChiTietDonHangs)
            {
                decimal donGia = ct.DonGia;
                dto.SanPhams.Add(new OrderItemDto
                {
                    STT = stt++,
                    TenThuoc = ct.Thuoc?.TenThuoc ?? "Sản phẩm không xác định",
                    HinhAnh = !string.IsNullOrEmpty(ct.Thuoc?.HinhAnh) ? ct.Thuoc.HinhAnh : DEFAULT_IMAGE,
                    DonGia = donGia,
                    SoLuong = ct.SoLuong,
                    ThanhTien = ct.ThanhTien ?? (donGia * ct.SoLuong)
                });
            }

            return new ApiResponse<OrderDetailDto> { Success = true, Message = "Thành công", Data = dto };
        }

        public async Task<ApiResponse<string>> UpdateOrderStatusAsync(Guid maDonHang, string newStatus, Guid userId)
        {
            var order = await _repo.GetOrderByIdAsync(maDonHang);
            if (order == null) 
                return new ApiResponse<string> { Success = false, Message = "Không tìm thấy đơn hàng." };

            // không cho thay đổi nếu đơn đã giao hoặc đã hủy
            if (order.TrangThai == "DaGiao" || order.TrangThai == "DaHuy")
                return new ApiResponse<string> { Success = false, Message = "Không thể thay đổi trạng thái của đơn hàng đã hoàn thành hoặc đã hủy." };

            // Gắn mã nhân viên xử lý
            var nhanVien = await _repo.GetNhanVienByUserIdAsync(userId);
            if (nhanVien == null) 
                return new ApiResponse<string> { Success = false, Message = "Tài khoản của bạn không có quyền nhân viên để thực hiện thao tác này." };

            if (order.TrangThai != newStatus)
            {
                order.TrangThai = newStatus;
                // Cập nhật ngày đặt/xử lý (theo logic hiện tại của bạn)
                order.NgayDat = DateTime.Now; 
                order.MaNhanVien = nhanVien.MaNhanVien;
                await _repo.SaveChangesAsync();
            }

            return new ApiResponse<string> { Success = true, Message = "Cập nhật trạng thái thành công." };
        }

        private string GetStatusLabel(string status) => status switch
        {
            "ChoXacNhan" => "Chờ xử lý",
            "DangXuLy" => "Đang xử lý",
            "DangGiao" => "Đang giao",
            "DaGiao" => "Hoàn thành",
            "DaHuy" => "Đã hủy",
            _ => status
        };

        private string GetStatusColor(string status) => status switch
        {
            "ChoXacNhan" => "#f59e0b",
            "DangXuLy" => "#3b82f6",
            "DangGiao" => "#0dcaf0",
            "DaGiao" => "#10b981",
            "DaHuy" => "#ef4444",
            _ => "#6b7280"
        };
    }
}