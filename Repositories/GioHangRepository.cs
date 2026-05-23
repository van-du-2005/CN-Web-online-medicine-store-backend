using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Repositories
{
    public class GioHangRepository : GenericRepository<GioHang>, IGioHangRepository
    {
        public GioHangRepository(OnlineMedicineStoreCNWDbContext context) : base(context) { }

        private async Task<Guid> GetMaKhachHangAsync(Guid maNguoiDung)
        {
            var khachHang = await _Context.KhachHangs
                .FirstOrDefaultAsync(k => k.MaNguoiDung == maNguoiDung);
            return khachHang?.MaKhachHang ?? Guid.Empty;
        }

        public async Task<GioHang?> GetCartWithDetailsAsync(Guid maNguoiDung)
        {
            var maKhachHang = await GetMaKhachHangAsync(maNguoiDung);
            if (maKhachHang == Guid.Empty) return null;

            return await _Context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .ThenInclude(c => c.Thuoc)
                .FirstOrDefaultAsync(g => g.MaKhachHang == maKhachHang);
        }

        public async Task AddCartItemAsync(Guid maNguoiDung, Guid maThuoc, int soLuong)
        {
            var maKhachHang = await GetMaKhachHangAsync(maNguoiDung);
            if (maKhachHang == Guid.Empty)
                throw new Exception("Không tìm thấy khách hàng.");

            var gioHang = await _Context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .FirstOrDefaultAsync(g => g.MaKhachHang == maKhachHang);

            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    MaGioHang = Guid.NewGuid(),
                    MaKhachHang = maKhachHang,
                    NgayTao = DateTime.UtcNow,
                    ChiTietGioHangs = new List<ChiTietGioHang>()
                };
                _Context.GioHangs.Add(gioHang);
                await _Context.SaveChangesAsync();
            }

            var chiTiet = gioHang.ChiTietGioHangs
                .FirstOrDefault(c => c.MaThuoc == maThuoc);

            if (chiTiet == null)
            {
                gioHang.ChiTietGioHangs.Add(new ChiTietGioHang
                {
                    MaThuoc = maThuoc,
                    SoLuong = soLuong,
                    NgayThem = DateTime.UtcNow
                });
            }
            else
            {
                chiTiet.SoLuong += soLuong;
            }

            await _Context.SaveChangesAsync();
        }

        public async Task UpdateCartItemQuantityAsync(Guid maNguoiDung, Guid maThuoc, int soLuong)
        {
            var maKhachHang = await GetMaKhachHangAsync(maNguoiDung);
            if (maKhachHang == Guid.Empty) return;

            var gioHang = await _Context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .FirstOrDefaultAsync(g => g.MaKhachHang == maKhachHang);

            if (gioHang != null)
            {
                var chiTiet = gioHang.ChiTietGioHangs
                    .FirstOrDefault(c => c.MaThuoc == maThuoc);
                if (chiTiet != null)
                {
                    chiTiet.SoLuong = soLuong;
                    await _Context.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveCartItemAsync(Guid maNguoiDung, Guid maThuoc)
        {
            var maKhachHang = await GetMaKhachHangAsync(maNguoiDung);
            if (maKhachHang == Guid.Empty) return;

            var chiTiet = await _Context.ChiTietGioHangs
                .Include(c => c.GioHang)
                .FirstOrDefaultAsync(c => c.GioHang.MaKhachHang == maKhachHang
                                       && c.MaThuoc == maThuoc);

            if (chiTiet != null)
            {
                _Context.ChiTietGioHangs.Remove(chiTiet);
                await _Context.SaveChangesAsync();
            }
        }
    }
}