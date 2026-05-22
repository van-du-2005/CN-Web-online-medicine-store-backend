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
            throw new Exception("Không tìm thấy dữ liệu hồ sơ thành viên. Vui lòng thử lại.");

        var maKhachHang = khachHang.MaKhachHang;

        // Chuẩn hóa địa chỉ chuỗi
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

        // FIX LOGIC GIÁ TRỊ: Tính toán tiền tạm tính chính xác từ danh sách sản phẩm thực tế thay vì trừ cứng số tiền
        decimal phiShip = 15000m;
        decimal tongTienThuoc = checkoutDTO.SanPhamDaMua.Sum(item => (decimal)item.SoLuong * (decimal)item.DonGia);

        var donhang = new DonHang
        {
            MaDonHang = Guid.NewGuid(),
            MaKhachHang = maKhachHang,
            MaDiaChi = diaChiMoi.MaDiaChi,
            LoaiDon = "Online",
            NgayDat = DateTime.Now,
            TamTinh = tongTienThuoc,
            PhiVanChuyen = phiShip,
            GiamGia = 0m,
            ThanhToan = tongTienThuoc + phiShip, // Tổng tiền cuối cùng bao gồm phí giao hàng
            PhuongThucThanhToan = checkoutDTO.PhuongThucThanhToan,
            TrangThai = checkoutDTO.PhuongThucThanhToan?.ToUpper() == "COD" ? "ChoXacNhan" : "ChoThanhToan",
        };

        await _unitOfWork.DonHangs.AddAsync(donhang);
        await _unitOfWork.SaveChangesAsync();

        foreach (var item in checkoutDTO.SanPhamDaMua)
        {
            var thuoc = await _unitOfWork.Thuocs.GetByIdAsync(item.MaThuoc);
            if (thuoc == null || !thuoc.TrangThai)
            {
                throw new Exception($"Dược phẩm '{item.TenThuoc}' hiện tại không tồn tại hoặc đã ngừng xuất bản.");
            }
            if (thuoc.TonKhoHienTai < item.SoLuong)
            {
                throw new Exception($"Số lượng tồn kho của thuốc '{thuoc.TenThuoc}' không đủ đáp ứng (Còn lại: {thuoc.TonKhoHienTai}).");
            }

            var tatCaLo = await _unitOfWork.LoThuocs.GetAllAsync();
            var today = DateOnly.FromDateTime(DateTime.Now);

            // Tìm kiếm lô thuốc có hạn dùng xa nhất và còn đủ hàng xuất bán
            var loPhuHop = tatCaLo.Where(l => l.MaThuoc == item.MaThuoc
                                           && l.SoLuongHienTai >= item.SoLuong
                                           && l.NgayHetHan > today)
                                  .OrderBy(l => l.NgayHetHan)
                                  .FirstOrDefault();

            if (loPhuHop == null)
            {
                throw new Exception($"Thuốc '{item.TenThuoc}' hiện không tìm thấy lô hàng nào đủ hạn sử dụng hợp lệ.");
            }

            // Trừ số lượng tồn kho tổng và tồn kho theo lô tương ứng
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
                DonGia = (decimal)item.DonGia
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