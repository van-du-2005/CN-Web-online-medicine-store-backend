using OnlineMedicineStoreBackend.DTOs;
using OnlineMedicineStoreBackend.Models;
using OnlineMedicineStoreBackend.Repositories;

namespace OnlineMedicineStoreBackend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DonHang> CreateOrderAsync(CheckoutDTO checkoutDTO)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var dsKhachHang = await _unitOfWork.KhachHangs.GetAllAsync();
                var khachHang = dsKhachHang.FirstOrDefault(k => k.MaNguoiDung == checkoutDTO.MaKhachHang);

                if (khachHang == null)
                    throw new Exception("Không tìm thấy thông tin khách hàng. Vui lòng đăng nhập lại.");

                var maKhachHang = khachHang.MaKhachHang;

                string diaChiDayDu = $"{checkoutDTO.DiaChiCuThe}, {checkoutDTO.Phuong}, {checkoutDTO.Tinh}";
                var diaChiMoi = new DiaChiKhachHang
                {
                    MaDiaChi = Guid.NewGuid(),
                    MaKhachHang = maKhachHang,
                    HoTenNguoiNhan = checkoutDTO.TenNguoiMua,
                    SDTNguoiNhan = checkoutDTO.SoDienThoai,
                    DiaChi = diaChiDayDu,
                };

                await _unitOfWork.DiaChiKhachHangs.AddAsync(diaChiMoi);
                await _unitOfWork.SaveChangesAsync();

                var donhang = new DonHang
                {
                    MaDonHang = Guid.NewGuid(),
                    MaKhachHang = maKhachHang,
                    MaDiaChi = diaChiMoi.MaDiaChi,
                    LoaiDon = "Online",
                    NgayDat = DateTime.Now,
                    TamTinh = checkoutDTO.TongTienThanhToan - 15000m,
                    PhiVanChuyen = 15000m,
                    GiamGia = 0m,
                    ThanhToan = checkoutDTO.TongTienThanhToan,
                    PhuongThucThanhToan = checkoutDTO.PhuongThucThanhToan,
                    TrangThai = checkoutDTO.PhuongThucThanhToan == "COD" ? "ChoXacNhan" : "ChoThanhToan",
                };

                await _unitOfWork.DonHangs.AddAsync(donhang);
                await _unitOfWork.SaveChangesAsync();

                foreach (var item in checkoutDTO.SanPhamDaMua)
                {
                    var thuoc = await _unitOfWork.Thuocs.GetByIdAsync(item.MaThuoc);
                    if (thuoc == null || !thuoc.TrangThai)
                    {
                        throw new Exception($"Sản phẩm '{item.TenThuoc}' không tồn tại hoặc đã ngừng bán.");
                    }
                    if (thuoc.TonKhoHienTai < item.SoLuong)
                    {
                        throw new Exception($"Rất tiếc, '{thuoc.TenThuoc}' chỉ còn {thuoc.TonKhoHienTai} hộp trong kho.");
                    }

                    var tatCaLo = await _unitOfWork.LoThuocs.GetAllAsync();
                    var today = DateOnly.FromDateTime(DateTime.Now);

                    var loPhuHop = tatCaLo.Where(l => l.MaThuoc == item.MaThuoc
                                                   && l.SoLuongHienTai >= item.SoLuong
                                                   && l.NgayHetHan > today)
                                          .OrderBy(l => l.NgayHetHan)
                                          .FirstOrDefault();

                    if (loPhuHop == null)
                    {
                        throw new Exception($"Thuốc '{item.TenThuoc}' hiện không có lô hàng nào đủ điều kiện xuất bán.");
                    }

                    thuoc.TonKhoHienTai -= item.SoLuong;
                    _unitOfWork.Thuocs.Update(thuoc);

                    loPhuHop.SoLuongHienTai -= item.SoLuong;
                    _unitOfWork.LoThuocs.Update(loPhuHop);

                    var chiTiet = new ChiTietDonHang
                    {
                        MaChiTiet = Guid.NewGuid(),
                        MaDonHang = donhang.MaDonHang,
                        MaThuoc = item.MaThuoc,
                        MaLo = loPhuHop.MaLo,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia
                    };
                    await _unitOfWork.ChiTietDonHangs.AddAsync(chiTiet);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return donhang;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> UpdatePaymentStatusAsync(Guid maDonHang, string trangThaiDonHang)
        {
            var donHang = await _unitOfWork.DonHangs.GetByIdAsync(maDonHang);

            if (donHang == null)
            {
                return false;
            }

            donHang.TrangThai = trangThaiDonHang;
            _unitOfWork.DonHangs.Update(donHang);

            int rows = await _unitOfWork.SaveChangesAsync();
            return rows > 0;
        }
    }
}