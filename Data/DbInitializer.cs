using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Models;
// Cần cài thư viện BCrypt.Net-Next qua NuGet nếu chưa cài để dùng BCrypt

namespace OnlineMedicineStoreBackend.Data
{
    public static class DbInitializer
    {
        public static async Task SeedDataAsync(OnlineMedicineStoreDbContext context)
        {
            // Tự động chạy Migration để tạo Database nếu chưa có
            await context.Database.MigrateAsync();

            if (await context.NguoiDungs.AnyAsync())
            {
                return;   // Thoát ngay lập tức
            }

            // Khởi tạo các khóa chính cố định để tham chiếu chéo
            Guid user1 = Guid.Parse("11111111-0000-0000-0000-000000000001");
            Guid user2 = Guid.Parse("11111111-0000-0000-0000-000000000002");
            Guid admin1 = Guid.Parse("11111111-0000-0000-0000-000000000003");
            Guid staff1 = Guid.Parse("11111111-0000-0000-0000-000000000004");

            Guid kh1 = Guid.Parse("22222222-0000-0000-0000-000000000001");
            Guid kh2 = Guid.Parse("22222222-0000-0000-0000-000000000002");

            Guid nv1 = Guid.Parse("33333333-0000-0000-0000-000000000001");
            Guid nv2 = Guid.Parse("33333333-0000-0000-0000-000000000002");

            Guid dm1 = Guid.Parse("DDDDDDDD-0000-0000-0000-000000000001");
            Guid dm2 = Guid.Parse("DDDDDDDD-0000-0000-0000-000000000002");
            Guid dm3 = Guid.Parse("DDDDDDDD-0000-0000-0000-000000000003");

            Guid ncc1 = Guid.Parse("CCCCCCCC-0000-0000-0000-000000000001");
            Guid ncc2 = Guid.Parse("CCCCCCCC-0000-0000-0000-000000000002");
            Guid ncc3 = Guid.Parse("CCCCCCCC-0000-0000-0000-000000000003");
            Guid ncc4 = Guid.Parse("CCCCCCCC-0000-0000-0000-000000000004");
            Guid ncc5 = Guid.Parse("CCCCCCCC-0000-0000-0000-000000000005");

            Guid pn1 = Guid.Parse("EEEEEEEE-0000-0000-0000-000000000001");
            Guid pn2 = Guid.Parse("EEEEEEEE-0000-0000-0000-000000000002");
            Guid pn3 = Guid.Parse("EEEEEEEE-0000-0000-0000-000000000003");

            Guid gh1 = Guid.Parse("FFFFFFFF-0000-0000-0000-000000000001");
            Guid gh2 = Guid.Parse("FFFFFFFF-0000-0000-0000-000000000002");

            // Danh sách ID Thuốc cần dùng cho liên kết Lô Thuốc và Giỏ Hàng
            Guid t1 = Guid.Parse("88888888-0000-0000-0000-000000000001");
            Guid t2 = Guid.Parse("88888888-0000-0000-0000-000000000002");
            Guid t3 = Guid.Parse("88888888-0000-0000-0000-000000000003");
            Guid t4 = Guid.Parse("88888888-0000-0000-0000-000000000004");
            Guid t5 = Guid.Parse("88888888-0000-0000-0000-000000000005");
            Guid t6 = Guid.Parse("88888888-0000-0000-0000-000000000006");

            // HASH MẬT KHẨU
            string defaultPasswordHash = BCrypt.Net.BCrypt.HashPassword("123456");
            
            // seed dữ liệu vào database
            // --- NguoiDung ---
            var nguoiDungs = new List<NguoiDung>
            {
                
                new NguoiDung { MaNguoiDung = user1, TenDangNhap = "nguyen_van_khach_hang_1", MatKhau = defaultPasswordHash, HoTen = "Nguyễn Văn Đức", Email = "nguyenvanduc@example.com", SoDienThoai = "0912345678", NgaySinh = new DateOnly(1990, 5, 15), GioiTinh = true /* Nam */, DiaChi = "123 Đường Lê Lợi, Tp.HCM", VaiTro = "KhachHang", NgayTao = new DateTime(2026, 4, 22, 15, 56, 55), TrangThai = true },
                new NguoiDung { MaNguoiDung = user2, TenDangNhap = "nguyen_van_khach_hang_2", MatKhau = defaultPasswordHash, HoTen = "Trần Quỳnh Phương", Email = "tranquynhphuong@example.com", SoDienThoai = "0987654321", NgaySinh = new DateOnly(1995, 8, 20), GioiTinh = false /* Nữ */, DiaChi = "456 Đường Nguyễn Huệ, Hà Nội", VaiTro = "KhachHang", NgayTao = new DateTime(2026, 4, 22, 15, 56, 56), TrangThai = true },
                new NguoiDung { MaNguoiDung = admin1, TenDangNhap = "nguyen_van_admin_1", MatKhau = defaultPasswordHash, HoTen = "Trần Hoàng Hải", Email = "hoanghai.manager@techmart.com", SoDienThoai = "0933112233", NgaySinh = new DateOnly(1985, 10, 10), GioiTinh = true /* Nam */, DiaChi = "789 Nguyễn Văn Linh, Quận 7, Tp.HCM", VaiTro = "Admin", NgayTao = new DateTime(2025, 1, 15, 8, 0, 0), TrangThai = true },
                new NguoiDung { MaNguoiDung = staff1, TenDangNhap = "NguyenVanAdmin_2", MatKhau = defaultPasswordHash, HoTen = "Lê Thị Thu", Email = "thu.le@techmart.com", SoDienThoai = "0944556677", NgaySinh = new DateOnly(1998, 3, 25), GioiTinh = false /* Nữ */, DiaChi = "101 Pasteur, Quận 3, Tp.HCM", VaiTro = "Admin", NgayTao = new DateTime(2025, 6, 20, 8, 30, 0), TrangThai = true }
            };

            // --- KhachHang---
            var khachHangs = new List<KhachHang>
            {
                new KhachHang { MaKhachHang = kh1, MaNguoiDung = user1, DiemTichLuy = 500, HangThanhVien = "Bạc" },
                new KhachHang { MaKhachHang = kh2, MaNguoiDung = user2, DiemTichLuy = 1000, HangThanhVien = "Vàng" }
                // ... 
            };


            // --- NhanVien---
            var nhanViens = new List<NhanVien>
            {
                new NhanVien { MaNhanVien = nv1, MaNguoiDung = admin1, ChucVu = "Admin", NgayVaoLam = new DateOnly(2025, 1, 16), Luong = 18000000m },
                new NhanVien { MaNhanVien = nv2, MaNguoiDung = staff1, ChucVu = "Dược sĩ", NgayVaoLam = new DateOnly(2025, 6, 21), Luong = 12000000m }
            };
            
            // --- DANH MỤC ---
            var danhMucs = new List<DanhMuc>
            {
                new DanhMuc { MaDanhMuc = dm1, TenDanhMuc = "Hạ sốt - Giảm đau" },
                new DanhMuc { MaDanhMuc = dm2, TenDanhMuc = "Vitamin - Thực phẩm chức năng" },
                new DanhMuc { MaDanhMuc = dm3, TenDanhMuc = "Tiêu hóa" }
            };

            // --- NHÀ CUNG CẤP ---
            var nhaCungCaps = new List<NhaCungCap>
            {
                new NhaCungCap { MaNhaCungCap = ncc1, TenCongTy = "Công ty TNHH Dược phẩm Vina", NguoiLienHe = "Trần Đình Trọng", SoDienThoai = "0901234567", Email = "contact@vinapharm.com.vn", TrangThai = true },
                new NhaCungCap { MaNhaCungCap = ncc2, TenCongTy = "Dược Hậu Giang (DHG Pharma)", NguoiLienHe = "Lê Thị Lan", SoDienThoai = "02923891433", Email = "info@dhgpharma.com.vn", TrangThai = true },
                new NhaCungCap { MaNhaCungCap = ncc3, TenCongTy = "Công ty CP Dược phẩm Trung ương 3", NguoiLienHe = "Nguyễn Quang Hải", SoDienThoai = "02253846624", Email = "sales@foripharm.vn", TrangThai = true },
                new NhaCungCap { MaNhaCungCap = ncc4, TenCongTy = "Công ty TNHH Sanofi-Aventis Việt Nam", NguoiLienHe = "Phạm Hoàng Yến", SoDienThoai = "02838298526", Email = "contact.vn@sanofi.com", TrangThai = true },
                new NhaCungCap { MaNhaCungCap = ncc5, TenCongTy = "Công ty Cổ phần Traphaco", NguoiLienHe = "Vũ Xuân Trường", SoDienThoai = "02437363015", Email = "info@traphaco.com.vn", TrangThai = true }

            };
           

            // --- PHIẾU NHẬP ---
            var phieuNhaps = new List<PhieuNhap>
            {
                new PhieuNhap { MaPhieuNhap = pn1, MaNhanVien = nv1, MaNhaCungCap = ncc1, NgayNhap = new DateTime(2026, 3, 10, 9, 30, 0), TongTien = 125000000m, GhiChu = "Nhập hàng chuẩn bị khai trương cửa hàng", TrangThai = "Hoàn thành" },
                new PhieuNhap { MaPhieuNhap = pn2, MaNhanVien = nv1, MaNhaCungCap = ncc2, NgayNhap = new DateTime(2026, 4, 5, 14, 0, 0), TongTien = 45500000m, GhiChu = "Nhập bổ sung nhóm thuốc hạ sốt, giảm đau", TrangThai = "Hoàn thành" },
                new PhieuNhap { MaPhieuNhap = pn3, MaNhanVien = nv2, MaNhaCungCap = ncc4, NgayNhap = new DateTime(2026, 5, 2, 10, 15, 0), TongTien = 88000000m, GhiChu = "Nhập kho thuốc tiêu hóa đợt đầu tháng 5", TrangThai = "Hoàn thành" }
            };

            // --- THUỐC (30 SẢN PHẨM) ---
            var thuocs = new List<Thuoc>
            {
                // DANH MỤC 1: HẠ SỐT - GIẢM ĐAU
                new Thuoc { MaThuoc = t1, MaDanhMuc = dm1, TenThuoc = "Paracetamol Stada 500mg", TenKhoaHoc = "Paracetamol", LoaiThuoc = "Viên nén", GiaBan = 50000m, MoTa = "Điều trị các cơn đau đầu, đau thần kinh, đau răng, hạ sốt", HuongDanSuDung = "Người lớn: 1-2 viên/lần, cách nhau 4-6 giờ. Không quá 8 viên/ngày", TonKhoHienTai = 9, HinhAnh = "/images/products/ha-sot-giam-dau/paracetamol-stada-500mg.jpg", TrangThai = true },
                new Thuoc { MaThuoc = t2, MaDanhMuc = dm1, TenThuoc = "Actadol 500mg Medipharco", TenKhoaHoc = "Paracetamol", LoaiThuoc = "Viên nén", GiaBan = 50000m, MoTa = "Điều trị các chứng đau và sốt từ nhẹ đến vừa", HuongDanSuDung = "Người lớn: 1-2 viên/lần, cách 4-6 giờ uống lại nếu cần", TonKhoHienTai = 150, HinhAnh = "/images/products/ha-sot-giam-dau/actadol-500mg.jpg", TrangThai = true },
                new Thuoc { MaThuoc = t3, MaDanhMuc = dm1, TenThuoc = "Bivinadol 500mg BRV", TenKhoaHoc = "Paracetamol", LoaiThuoc = "Viên sủi bọt", GiaBan = 32000m, MoTa = "Giảm đau, hạ sốt dạng sủi bọt dễ uống", HuongDanSuDung = "Hòa tan 1 viên vào 200ml nước, uống ngay. Cách 4-6 giờ uống lại", TonKhoHienTai = 100, HinhAnh = "/images/products/ha-sot-giam-dau/bivinadol-500mg.jpg", TrangThai = true },
                new Thuoc { MaThuoc = t4, MaDanhMuc = dm1, TenThuoc = "Glotadol 325 Abbott", TenKhoaHoc = "Paracetamol", LoaiThuoc = "Viên nén", GiaBan = 90000m, MoTa = "Hỗ trợ hạ sốt và giảm đau, lọ 200 viên tiện dùng", HuongDanSuDung = "Người lớn: 2 viên/lần, cách nhau 4-6 giờ. Tối đa 8 viên/ngày", TonKhoHienTai = 80, HinhAnh = "/images/products/ha-sot-giam-dau/glotadol-325.jpg", TrangThai = true },
                new Thuoc { MaThuoc = t5, MaDanhMuc = dm1, TenThuoc = "Glotadol 650 Abbott", TenKhoaHoc = "Paracetamol", LoaiThuoc = "Viên nén", GiaBan = 130000m, MoTa = "Hỗ trợ hạ sốt và giảm đau liều 650mg, lọ 200 viên", HuongDanSuDung = "Người lớn: 1 viên/lần, cách nhau 4-6 giờ. Không quá 6 viên/ngày", TonKhoHienTai = 60, HinhAnh = "/images/products/ha-sot-giam-dau/glotadol-650.jpg", TrangThai = true },
                new Thuoc { MaThuoc = t6, MaDanhMuc = dm1, TenThuoc = "Panactol 500mg Khapharco", TenKhoaHoc = "Paracetamol", LoaiThuoc = "Viên nén", GiaBan = 40000m, MoTa = "Giảm đau, hạ sốt, điều trị đau đầu, đau răng", HuongDanSuDung = "Người lớn: 1-2 viên/lần, 3-4 lần/ngày, cách nhau ít nhất 4 giờ", TonKhoHienTai = 120, HinhAnh = "/images/products/ha-sot-giam-dau/panactol-500mg.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000007"), MaDanhMuc = dm1, TenThuoc = "Panactol Extra Khapharco", TenKhoaHoc = "Paracetamol + Caffeine", LoaiThuoc = "Viên nén", GiaBan = 60000m, MoTa = "Hạ sốt, giảm đau tăng cường với Caffeine", HuongDanSuDung = "Người lớn: 1-2 viên/lần, tối đa 6 viên/ngày", TonKhoHienTai = 90, HinhAnh = "/images/products/ha-sot-giam-dau/panactol-extra.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000008"), MaDanhMuc = dm1, TenThuoc = "Glotadol 150 Abbott", TenKhoaHoc = "Paracetamol", LoaiThuoc = "Bột pha uống", GiaBan = 44000m, MoTa = "Hỗ trợ hạ sốt và giảm đau cho trẻ em dạng bột dễ uống", HuongDanSuDung = "Hòa tan 1 gói vào 10-20ml nước, dùng cho trẻ theo cân nặng", TonKhoHienTai = 70, HinhAnh = "/images/products/ha-sot-giam-dau/glotadol-150-bot.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000009"), MaDanhMuc = dm1, TenThuoc = "Tovalgan Ef 500mg Trường Thọ", TenKhoaHoc = "Paracetamol", LoaiThuoc = "Viên sủi", GiaBan = 40000m, MoTa = "Giảm đau, hạ sốt dạng viên sủi hòa tan nhanh", HuongDanSuDung = "Hòa tan 1 viên vào ly nước, uống ngay khi sủi tan hoàn toàn", TonKhoHienTai = 85, HinhAnh = "/images/products/ha-sot-giam-dau/tovalgan-ef-500mg.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000010"), MaDanhMuc = dm1, TenThuoc = "Ameflu Không Gây Buồn Ngủ OPV", TenKhoaHoc = "Paracetamol + Phenylephrine", LoaiThuoc = "Viên nén", GiaBan = 110000m, MoTa = "Điều trị các triệu chứng cảm lạnh, cảm cúm, không gây buồn ngủ", HuongDanSuDung = "Người lớn: 1-2 viên/lần, 3-4 lần/ngày", TonKhoHienTai = 110, HinhAnh = "/images/products/ha-sot-giam-dau/ameflu-khong-gay-buon-ngu.jpg", TrangThai = true },

                // DANH MỤC 2: VITAMIN - THỰC PHẨM CHỨC NĂNG
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000011"), MaDanhMuc = dm2, TenThuoc = "Vitamin C 1000mg DHG", TenKhoaHoc = "Ascorbic Acid", LoaiThuoc = "Viên sủi", GiaBan = 85000m, MoTa = "Bổ sung Vitamin C, tăng cường đề kháng, chống oxy hóa", HuongDanSuDung = "Hòa tan 1 viên vào 200ml nước, uống 1 lần/ngày sau bữa ăn", TonKhoHienTai = 150, HinhAnh = "/images/products/vitamin/vitamin-c-1000mg-dhg.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000012"), MaDanhMuc = dm2, TenThuoc = "Vitamin D3 2000IU Blackmores", TenKhoaHoc = "Cholecalciferol", LoaiThuoc = "Viên nang mềm", GiaBan = 320000m, MoTa = "Hỗ trợ hấp thu canxi, tăng cường xương khớp và miễn dịch", HuongDanSuDung = "Uống 1 viên/ngày trong bữa ăn", TonKhoHienTai = 80, HinhAnh = "/images/products/vitamin/vitamin-d3-blackmores.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000013"), MaDanhMuc = dm2, TenThuoc = "Omega-3 Fish Oil Nature Made", TenKhoaHoc = "EPA + DHA", LoaiThuoc = "Viên nang mềm", GiaBan = 280000m, MoTa = "Bổ sung Omega-3, hỗ trợ tim mạch và não bộ", HuongDanSuDung = "Uống 1-2 viên/ngày cùng bữa ăn", TonKhoHienTai = 90, HinhAnh = "/images/products/vitamin/omega-3-nature-made.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000014"), MaDanhMuc = dm2, TenThuoc = "Canxi D3 Ostelin", TenKhoaHoc = "Calcium + Vitamin D3", LoaiThuoc = "Viên nén", GiaBan = 195000m, MoTa = "Bổ sung canxi và vitamin D3, hỗ trợ xương chắc khỏe", HuongDanSuDung = "Uống 1 viên/ngày sau bữa ăn", TonKhoHienTai = 100, HinhAnh = "/images/products/vitamin/canxi-d3-ostelin.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000015"), MaDanhMuc = dm2, TenThuoc = "Kẽm Gluconate Biovital 10mg", TenKhoaHoc = "Zinc Gluconate", LoaiThuoc = "Viên nén", GiaBan = 75000m, MoTa = "Bổ sung kẽm, hỗ trợ miễn dịch và phát triển cơ thể", HuongDanSuDung = "Người lớn: 1-2 viên/ngày sau bữa ăn", TonKhoHienTai = 130, HinhAnh = "/images/products/vitamin/kem-gluconate-10mg.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000016"), MaDanhMuc = dm2, TenThuoc = "Vitamin tổng hợp Centrum Adults", TenKhoaHoc = "Multivitamin", LoaiThuoc = "Viên nén", GiaBan = 350000m, MoTa = "Bổ sung đầy đủ vitamin và khoáng chất cho người trưởng thành", HuongDanSuDung = "Uống 1 viên/ngày sau bữa ăn sáng", TonKhoHienTai = 70, HinhAnh = "/images/products/vitamin/centrum-adults.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000017"), MaDanhMuc = dm2, TenThuoc = "Magnesium B6 Doppelherz", TenKhoaHoc = "Magnesium + Vitamin B6", LoaiThuoc = "Viên nén", GiaBan = 165000m, MoTa = "Bổ sung magie và B6, giảm mệt mỏi, hỗ trợ thần kinh", HuongDanSuDung = "Uống 1 viên/ngày sau bữa ăn tối", TonKhoHienTai = 85, HinhAnh = "/images/products/vitamin/magnesium-b6-doppelherz.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000018"), MaDanhMuc = dm2, TenThuoc = "Sắt Hữu Cơ Ferrous Fumarate", TenKhoaHoc = "Ferrous Fumarate", LoaiThuoc = "Viên nang", GiaBan = 95000m, MoTa = "Bổ sung sắt hữu cơ, hỗ trợ phòng ngừa thiếu máu", HuongDanSuDung = "Uống 1 viên/ngày trước bữa ăn với nước lọc", TonKhoHienTai = 110, HinhAnh = "/images/products/vitamin/sat-huu-co-ferrous-fumarate.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000019"), MaDanhMuc = dm2, TenThuoc = "Vitamin B Complex DHG", TenKhoaHoc = "Vitamin B1, B2, B6, B12", LoaiThuoc = "Viên nén", GiaBan = 45000m, MoTa = "Bổ sung vitamin nhóm B, hỗ trợ chuyển hóa năng lượng", HuongDanSuDung = "Uống 1-2 viên/ngày sau bữa ăn", TonKhoHienTai = 160, HinhAnh = "/images/products/vitamin/vitamin-b-complex-dhg.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000020"), MaDanhMuc = dm2, TenThuoc = "Men vi sinh Enterogermina", TenKhoaHoc = "Bacillus clausii", LoaiThuoc = "Dung dịch uống", GiaBan = 120000m, MoTa = "Cân bằng hệ vi sinh đường ruột, hỗ trợ tiêu hóa", HuongDanSuDung = "Người lớn: 2-3 ống/ngày. Trẻ em: 1-2 ống/ngày", TonKhoHienTai = 95, HinhAnh = "/images/products/vitamin/enterogermina.jpg", TrangThai = true },

                // DANH MỤC 3: TIÊU HÓA
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000021"), MaDanhMuc = dm3, TenThuoc = "Smecta 3g Ipsen", TenKhoaHoc = "Diosmectite", LoaiThuoc = "Bột pha uống", GiaBan = 85000m, MoTa = "Điều trị tiêu chảy cấp và mãn tính, bảo vệ niêm mạc ruột", HuongDanSuDung = "Pha 1 gói vào 50ml nước, uống 3 lần/ngày trước bữa ăn", TonKhoHienTai = 120, HinhAnh = "/images/products/tieu-hoa/smecta-3g.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000022"), MaDanhMuc = dm3, TenThuoc = "Oresol bù nước và điện giải", TenKhoaHoc = "ORS (Glucose + NaCl + KCl)", LoaiThuoc = "Bột pha uống", GiaBan = 15000m, MoTa = "Bù nước và điện giải khi tiêu chảy, nôn ói, sốt cao", HuongDanSuDung = "Pha 1 gói vào 200ml nước sôi để nguội, uống từng ngụm nhỏ", TonKhoHienTai = 200, HinhAnh = "/images/products/tieu-hoa/oresol.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000023"), MaDanhMuc = dm3, TenThuoc = "Omeprazole 20mg Stada", TenKhoaHoc = "Omeprazole", LoaiThuoc = "Viên nang", GiaBan = 65000m, MoTa = "Điều trị loét dạ dày tá tràng, trào ngược dạ dày thực quản", HuongDanSuDung = "Uống 1 viên/ngày trước bữa ăn sáng 30 phút", TonKhoHienTai = 90, HinhAnh = "/images/products/tieu-hoa/omeprazole-20mg-stada.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000024"), MaDanhMuc = dm3, TenThuoc = "Phosphalugel Aventis", TenKhoaHoc = "Aluminium Phosphate", LoaiThuoc = "Gel uống", GiaBan = 95000m, MoTa = "Trung hòa acid dạ dày, điều trị đau dạ dày, ợ chua", HuongDanSuDung = "Uống 1-2 gói/lần, 2-3 lần/ngày sau bữa ăn và trước khi ngủ", TonKhoHienTai = 75, HinhAnh = "/images/products/tieu-hoa/phosphalugel.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000025"), MaDanhMuc = dm3, TenThuoc = "Duphalac Solvay 200ml", TenKhoaHoc = "Lactulose", LoaiThuoc = "Siro", GiaBan = 110000m, MoTa = "Điều trị táo bón, làm mềm phân, hỗ trợ nhuận tràng", HuongDanSuDung = "Người lớn: 15-45ml/ngày. Trẻ em: tùy lứa tuổi theo hướng dẫn", TonKhoHienTai = 60, HinhAnh = "/images/products/tieu-hoa/duphalac-200ml.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000026"), MaDanhMuc = dm3, TenThuoc = "Berberin 50mg DHG", TenKhoaHoc = "Berberine Chloride", LoaiThuoc = "Viên nén", GiaBan = 25000m, MoTa = "Điều trị tiêu chảy, viêm ruột do vi khuẩn", HuongDanSuDung = "Người lớn: 2-4 viên/lần, 3 lần/ngày sau bữa ăn", TonKhoHienTai = 180, HinhAnh = "/images/products/tieu-hoa/berberin-50mg-dhg.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000027"), MaDanhMuc = dm3, TenThuoc = "Motilium-M 10mg Janssen", TenKhoaHoc = "Domperidone", LoaiThuoc = "Viên nén", GiaBan = 75000m, MoTa = "Điều trị buồn nôn, nôn ói, đầy bụng khó tiêu", HuongDanSuDung = "Uống 1 viên 3 lần/ngày, 15-30 phút trước bữa ăn", TonKhoHienTai = 85, HinhAnh = "/images/products/tieu-hoa/motilium-m-10mg.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000028"), MaDanhMuc = dm3, TenThuoc = "Imodium 2mg Janssen", TenKhoaHoc = "Loperamide HCl", LoaiThuoc = "Viên nang", GiaBan = 55000m, MoTa = "Điều trị tiêu chảy cấp và mãn tính nhanh chóng", HuongDanSuDung = "Người lớn: 2 viên ban đầu, sau đó 1 viên sau mỗi lần tiêu chảy", TonKhoHienTai = 95, HinhAnh = "/images/products/tieu-hoa/imodium-2mg.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000029"), MaDanhMuc = dm3, TenThuoc = "Ganaton 50mg ItalFarmaco", TenKhoaHoc = "Itopride HCl", LoaiThuoc = "Viên nén bao phim", GiaBan = 145000m, MoTa = "Điều trị các triệu chứng khó tiêu, đầy hơi, buồn nôn", HuongDanSuDung = "Uống 1 viên 3 lần/ngày trước bữa ăn", TonKhoHienTai = 65, HinhAnh = "/images/products/tieu-hoa/ganaton-50mg.jpg", TrangThai = true },
                new Thuoc { MaThuoc = Guid.Parse("88888888-0000-0000-0000-000000000030"), MaDanhMuc = dm3, TenThuoc = "Activated Charcoal 250mg", TenKhoaHoc = "Carbo Activatus", LoaiThuoc = "Viên nang", GiaBan = 35000m, MoTa = "Hấp phụ độc chất, hỗ trợ điều trị ngộ độc thức ăn, đầy hơi", HuongDanSuDung = "Người lớn: 2-4 viên/lần, 3 lần/ngày sau bữa ăn", TonKhoHienTai = 140, HinhAnh = "/images/products/tieu-hoa/activated-charcoal-250mg.jpg", TrangThai = true }
            };

            // --- Lo thuốc ---
            var loThuocs = new List<LoThuoc>
            {
                new LoThuoc { MaLo = Guid.Parse("99999999-0000-0000-0000-000000000001"), MaThuoc = t1, MaPhieuNhap = pn1, MaNhaCungCap = ncc1, NgaySanXuat = new DateOnly(2025, 12, 1), NgayHetHan = new DateOnly(2028, 12, 1), SoLuongNhap = 500, SoLuongHienTai = 200, GiaNhap = 30000m },
                new LoThuoc { MaLo = Guid.Parse("99999999-0000-0000-0000-000000000002"), MaThuoc = t2, MaPhieuNhap = pn1, MaNhaCungCap = ncc1, NgaySanXuat = new DateOnly(2026, 1, 10), NgayHetHan = new DateOnly(2029, 1, 10), SoLuongNhap = 300, SoLuongHienTai = 150, GiaNhap = 35000m },
                new LoThuoc { MaLo = Guid.Parse("99999999-0000-0000-0000-000000000003"), MaThuoc = t3, MaPhieuNhap = pn2, MaNhaCungCap = ncc2, NgaySanXuat = new DateOnly(2026, 2, 20), NgayHetHan = new DateOnly(2029, 2, 20), SoLuongNhap = 200, SoLuongHienTai = 100, GiaNhap = 20000m },
                new LoThuoc { MaLo = Guid.Parse("99999999-0000-0000-0000-000000000004"), MaThuoc = t4, MaPhieuNhap = pn2, MaNhaCungCap = ncc2, NgaySanXuat = new DateOnly(2026, 3, 5), NgayHetHan = new DateOnly(2028, 3, 5), SoLuongNhap = 150, SoLuongHienTai = 80, GiaNhap = 65000m },
                new LoThuoc { MaLo = Guid.Parse("99999999-0000-0000-0000-000000000005"), MaThuoc = t5, MaPhieuNhap = pn3, MaNhaCungCap = ncc4, NgaySanXuat = new DateOnly(2026, 4, 1), NgayHetHan = new DateOnly(2029, 4, 1), SoLuongNhap = 100, SoLuongHienTai = 60, GiaNhap = 95000m },
                new LoThuoc { MaLo = Guid.Parse("99999999-0000-0000-0000-000000000006"), MaThuoc = t6, MaPhieuNhap = pn3, MaNhaCungCap = ncc4, NgaySanXuat = new DateOnly(2026, 4, 15), NgayHetHan = new DateOnly(2028, 10, 15), SoLuongNhap = 200, SoLuongHienTai = 120, GiaNhap = 25000m }

            };

            // --- GIỎ HÀNG ---
            var gioHangs = new List<GioHang>
            {
                new GioHang { MaGioHang = gh1, MaKhachHang = kh1, NgayTao = new DateTime(2026, 4, 22, 15, 56, 57) },
                new GioHang { MaGioHang = gh2, MaKhachHang = kh2, NgayTao = new DateTime(2026, 4, 22, 15, 56, 58) }
            };

            // --- CHI TIẾT GIỎ HÀNG ---
            var chiTietGioHangs = new List<ChiTietGioHang>
            {
                // Khách hàng 1
                new ChiTietGioHang { MaChiTiet = Guid.Parse("AAAAAAAA-0000-0000-0000-000000000001"), MaGioHang = gh1, MaThuoc = t1, SoLuong = 2, NgayThem = new DateTime(2026, 4, 22, 15, 56, 57) },
                new ChiTietGioHang { MaChiTiet = Guid.Parse("AAAAAAAA-0000-0000-0000-000000000002"), MaGioHang = gh1, MaThuoc = t4, SoLuong = 1, NgayThem = new DateTime(2026, 4, 22, 15, 56, 58) },
                new ChiTietGioHang { MaChiTiet = Guid.Parse("AAAAAAAA-0000-0000-0000-000000000003"), MaGioHang = gh1, MaThuoc = t6, SoLuong = 3, NgayThem = new DateTime(2026, 4, 22, 15, 56, 59) },
                // Khách hàng 2
                new ChiTietGioHang { MaChiTiet = Guid.Parse("AAAAAAAA-0000-0000-0000-000000000004"), MaGioHang = gh2, MaThuoc = t2, SoLuong = 1, NgayThem = new DateTime(2026, 4, 22, 15, 57, 0) },
                new ChiTietGioHang { MaChiTiet = Guid.Parse("AAAAAAAA-0000-0000-0000-000000000005"), MaGioHang = gh2, MaThuoc = t3, SoLuong = 2, NgayThem = new DateTime(2026, 4, 22, 15, 57, 1) },
                new ChiTietGioHang { MaChiTiet = Guid.Parse("AAAAAAAA-0000-0000-0000-000000000006"), MaGioHang = gh2, MaThuoc = t5, SoLuong = 1, NgayThem = new DateTime(2026, 4, 22, 15, 57, 2) }

            };


            // ==========================================
            // THÊM MỚI: ĐỊA CHỈ, ĐƠN HÀNG, CHI TIẾT ĐƠN HÀNG
            // ==========================================

            // Khai báo các Guid cần thiết
            Guid dc1 = Guid.Parse("D1AC4176-2046-474E-AE14-9F16664C2182"); 
            Guid dc2 = Guid.Parse("D1AC414F-B141-46DE-9E7E-E4A5DEA757D5");

            // Khai báo 10 Guid cho Đơn hàng
            Guid dh1 = Guid.Parse("DDDDDDDD-1111-0000-0000-000000000001");
            Guid dh2 = Guid.Parse("DDDDDDDD-1111-0000-0000-000000000002");
            Guid dh3 = Guid.Parse("DDDDDDDD-1111-0000-0000-000000000003");
            Guid dh4 = Guid.Parse("DDDDDDDD-1111-0000-0000-000000000004");
            Guid dh5 = Guid.Parse("DDDDDDDD-1111-0000-0000-000000000005");
            Guid dh6 = Guid.Parse("DDDDDDDD-1111-0000-0000-000000000006");
            Guid dh7 = Guid.Parse("DDDDDDDD-1111-0000-0000-000000000007");
            Guid dh8 = Guid.Parse("DDDDDDDD-1111-0000-0000-000000000008");
            Guid dh9 = Guid.Parse("DDDDDDDD-1111-0000-0000-000000000009");
            Guid dh10 = Guid.Parse("DDDDDDDD-1111-0000-0000-000000000010");

            // Lấy lại Guid của Lô thuốc từ dữ liệu cũ để nối đúng
            Guid lo1 = Guid.Parse("99999999-0000-0000-0000-000000000001");
            Guid lo2 = Guid.Parse("99999999-0000-0000-0000-000000000002");
            Guid lo3 = Guid.Parse("99999999-0000-0000-0000-000000000003");
            Guid lo4 = Guid.Parse("99999999-0000-0000-0000-000000000004");
            Guid lo6 = Guid.Parse("99999999-0000-0000-0000-000000000006");

            // --- Dia Chi Khach Hang ---
            var diaChiKhachHangs = new List<DiaChiKhachHang>
            {
                new DiaChiKhachHang { MaDiaChi = dc1, MaKhachHang = kh1, HoTenNguoiNhan = "Nguyễn Văn Test 1", SDTNguoiNhan = "0123456789", DiaChi = "113 Khiếu Năng Tĩnh, An Lạc A, Bình Tân, TP.HCM", LaMacDinh = false },
                new DiaChiKhachHang { MaDiaChi = dc2, MaKhachHang = kh2, HoTenNguoiNhan = "Nguyễn Văn Test 2", SDTNguoiNhan = "0123456788", DiaChi = "113 Khiếu Năng Tĩnh, An Lạc, Bình Tân, TP.HCM", LaMacDinh = false }
            };
            

            // --- Don Hang ---
            var donHangs = new List<DonHang>
            {
                // 5 Đơn đầu tiên (Khách 1, Địa chỉ 1)
                new DonHang { MaDonHang = dh1, MaKhachHang = kh1, MaDiaChi = dc1, MaNhanVien = nv1, NgayDat = new DateTime(2026, 5, 1, 8, 30, 0), LoaiDon = "Trực tuyến", TamTinh = 150000, PhiVanChuyen = 30000, GiamGia = 0, ThanhToan = 180000, PhuongThucThanhToan = "Tiền mặt", TrangThai = "ChoXacNhan" },
                new DonHang { MaDonHang = dh2, MaKhachHang = kh1, MaDiaChi = dc1, MaNhanVien = nv1, NgayDat = new DateTime(2026, 5, 2, 9, 15, 0), LoaiDon = "Trực tuyến", TamTinh = 220000, PhiVanChuyen = 30000, GiamGia = 0, ThanhToan = 250000, PhuongThucThanhToan = "Chuyển khoản", TrangThai = "DangXuLy" },
                new DonHang { MaDonHang = dh3, MaKhachHang = kh1, MaDiaChi = dc1, MaNhanVien = nv1, NgayDat = new DateTime(2026, 5, 3, 10, 0, 0), LoaiDon = "Trực tuyến", TamTinh = 135000, PhiVanChuyen = 30000, GiamGia = 0, ThanhToan = 165000, PhuongThucThanhToan = "Tiền mặt", TrangThai = "DaGiao" },
                new DonHang { MaDonHang = dh4, MaKhachHang = kh1, MaDiaChi = dc1, MaNhanVien = nv1, NgayDat = new DateTime(2026, 5, 4, 14, 20, 0), LoaiDon = "Trực tuyến", TamTinh = 310000, PhiVanChuyen = 15000, GiamGia = 10000, ThanhToan = 315000, PhuongThucThanhToan = "Chuyển khoản", TrangThai = "DangGiao" },
                new DonHang { MaDonHang = dh5, MaKhachHang = kh1, MaDiaChi = dc1, MaNhanVien = nv1, NgayDat = new DateTime(2026, 5, 5, 16, 45, 0), LoaiDon = "Trực tuyến", TamTinh = 90000, PhiVanChuyen = 30000, GiamGia = 0, ThanhToan = 120000, PhuongThucThanhToan = "Tiền mặt", TrangThai = "DaGiao" },

                // 5 Đơn tiếp theo (Khách 2, Địa chỉ 2)
                new DonHang { MaDonHang = dh6, MaKhachHang = kh2, MaDiaChi = dc2, MaNhanVien = nv1, NgayDat = new DateTime(2026, 5, 1, 7, 0, 0), LoaiDon = "Trực tuyến", TamTinh = 185000, PhiVanChuyen = 30000, GiamGia = 0, ThanhToan = 215000, PhuongThucThanhToan = "Chuyển khoản", TrangThai = "DaHuy" },
                new DonHang { MaDonHang = dh7, MaKhachHang = kh2, MaDiaChi = dc2, MaNhanVien = nv1, NgayDat = new DateTime(2026, 5, 2, 11, 30, 0), LoaiDon = "Trực tuyến", TamTinh = 400000, PhiVanChuyen = 0, GiamGia = 20000, ThanhToan = 380000, PhuongThucThanhToan = "Chuyển khoản", TrangThai = "DaGiao" }, // Đổi Hoàn thành -> DaGiao để khớp logic Backend
                new DonHang { MaDonHang = dh8, MaKhachHang = kh2, MaDiaChi = dc2, MaNhanVien = nv1, NgayDat = new DateTime(2026, 5, 3, 13, 10, 0), LoaiDon = "Trực tuyến", TamTinh = 120000, PhiVanChuyen = 30000, GiamGia = 0, ThanhToan = 150000, PhuongThucThanhToan = "Tiền mặt", TrangThai = "ChoXacNhan" },
                new DonHang { MaDonHang = dh9, MaKhachHang = kh2, MaDiaChi = dc2, MaNhanVien = nv1, NgayDat = new DateTime(2026, 5, 4, 15, 55, 0), LoaiDon = "Trực tuyến", TamTinh = 255000, PhiVanChuyen = 30000, GiamGia = 0, ThanhToan = 285000, PhuongThucThanhToan = "Chuyển khoản", TrangThai = "ChoXacNhan" },
                new DonHang { MaDonHang = dh10, MaKhachHang = kh2, MaDiaChi = dc2, MaNhanVien = nv1, NgayDat = new DateTime(2026, 5, 5, 19, 20, 0), LoaiDon = "Trực tuyến", TamTinh = 330000, PhiVanChuyen = 30000, GiamGia = 15000, ThanhToan = 345000, PhuongThucThanhToan = "Tiền mặt", TrangThai = "ChoXacNhan" }
            };

            

            // --- Chi Tiết Đơn Hàng ---
            var chiTietDonHangs = new List<ChiTietDonHang>
            {
                // Đơn 1
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000001"), MaDonHang = dh1, MaThuoc = t1, MaLo = lo1, SoLuong = 2, DonGia = 50000m },
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000002"), MaDonHang = dh1, MaThuoc = t2, MaLo = lo2, SoLuong = 1, DonGia = 50000m },
                // Đơn 2
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000003"), MaDonHang = dh2, MaThuoc = t4, MaLo = lo4, SoLuong = 1, DonGia = 120000m },
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000004"), MaDonHang = dh2, MaThuoc = t6, MaLo = lo6, SoLuong = 2, DonGia = 50000m },
                // Đơn 3
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000005"), MaDonHang = dh3, MaThuoc = t1, MaLo = lo1, SoLuong = 1, DonGia = 50000m },
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000006"), MaDonHang = dh3, MaThuoc = t4, MaLo = lo4, SoLuong = 1, DonGia = 85000m },
                // Đơn 4
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000007"), MaDonHang = dh4, MaThuoc = t3, MaLo = lo3, SoLuong = 5, DonGia = 30000m },
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000008"), MaDonHang = dh4, MaThuoc = t4, MaLo = lo4, SoLuong = 1, DonGia = 160000m },
                // Đơn 5
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000009"), MaDonHang = dh5, MaThuoc = t1, MaLo = lo1, SoLuong = 1, DonGia = 50000m },
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000010"), MaDonHang = dh5, MaThuoc = t6, MaLo = lo6, SoLuong = 1, DonGia = 40000m },
                // Đơn 6
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000011"), MaDonHang = dh6, MaThuoc = t3, MaLo = lo3, SoLuong = 2, DonGia = 85000m },
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000012"), MaDonHang = dh6, MaThuoc = t2, MaLo = lo2, SoLuong = 1, DonGia = 15000m },
                // Đơn 7
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000013"), MaDonHang = dh7, MaThuoc = t4, MaLo = lo4, SoLuong = 2, DonGia = 150000m },
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000014"), MaDonHang = dh7, MaThuoc = t6, MaLo = lo6, SoLuong = 2, DonGia = 50000m },
                // Đơn 8
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000015"), MaDonHang = dh8, MaThuoc = t3, MaLo = lo3, SoLuong = 2, DonGia = 30000m },
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000016"), MaDonHang = dh8, MaThuoc = t1, MaLo = lo1, SoLuong = 1, DonGia = 60000m },
                // Đơn 9
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000017"), MaDonHang = dh9, MaThuoc = t6, MaLo = lo6, SoLuong = 3, DonGia = 85000m },
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000018"), MaDonHang = dh9, MaThuoc = t2, MaLo = lo2, SoLuong = 1, DonGia = 5000m },
                // Đơn 10
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000019"), MaDonHang = dh10, MaThuoc = t4, MaLo = lo4, SoLuong = 2, DonGia = 140000m },
                new ChiTietDonHang { MaChiTiet = Guid.Parse("CCCCCCCC-1111-0000-0000-000000000020"), MaDonHang = dh10, MaThuoc = t6, MaLo = lo6, SoLuong = 1, DonGia = 50000m }
            };
            

            //  Thêm dữ liệu vào Context (Dùng AddRange)
            await context.NguoiDungs.AddRangeAsync(nguoiDungs);
            await context.KhachHangs.AddRangeAsync(khachHangs);
            await context.NhanViens.AddRangeAsync(nhanViens);
            await context.NhaCungCaps.AddRangeAsync(nhaCungCaps);
            await context.DanhMucs.AddRangeAsync(danhMucs);
            await context.PhieuNhaps.AddRangeAsync(phieuNhaps);
            await context.Thuocs.AddRangeAsync(thuocs);
            await context.LoThuocs.AddRangeAsync(loThuocs);
            await context.GioHangs.AddRangeAsync(gioHangs);
            await context.ChiTietGioHangs.AddRangeAsync(chiTietGioHangs);
            await context.DiaChiKhachHangs.AddRangeAsync(diaChiKhachHangs);
            await context.DonHangs.AddRangeAsync(donHangs);
            await context.ChiTietDonHangs.AddRangeAsync(chiTietDonHangs);
            
            // -- kiểm trả nếu bảng nào chưa có dữ liệu thì mới thêm --
            if (!await context.NguoiDungs.AnyAsync())
            {
                await context.NguoiDungs.AddRangeAsync(nguoiDungs);
            }

            if (!await context.KhachHangs.AnyAsync())
            {
                await context.KhachHangs.AddRangeAsync(khachHangs);
            }

            if (!await context.NhanViens.AnyAsync())
            {
                await context.NhanViens.AddRangeAsync(nhanViens);
            }

            if (!await context.DanhMucs.AnyAsync())
            {
                await context.DanhMucs.AddRangeAsync(danhMucs);
            }

            if (!await context.NhaCungCaps.AnyAsync())
            {
                await context.NhaCungCaps.AddRangeAsync(nhaCungCaps);
            }

            if (!await context.PhieuNhaps.AnyAsync())
            {
                await context.PhieuNhaps.AddRangeAsync(phieuNhaps);
            }

            if (!await context.Thuocs.AnyAsync())
            {
                await context.Thuocs.AddRangeAsync(thuocs);
            }

            if (!await context.LoThuocs.AnyAsync())
            {
                await context.LoThuocs.AddRangeAsync(loThuocs);
            }

            if (!await context.GioHangs.AnyAsync())
            {
                await context.GioHangs.AddRangeAsync(gioHangs);
            }

            if (!await context.ChiTietGioHangs.AnyAsync())
            {
                await context.ChiTietGioHangs.AddRangeAsync(chiTietGioHangs);
            }

            if (!await context.DiaChiKhachHangs.AnyAsync())
            {
                await context.DiaChiKhachHangs.AddRangeAsync(diaChiKhachHangs);
            }

            if (!await context.DonHangs.AnyAsync())
            {
                await context.DonHangs.AddRangeAsync(donHangs);
            }

            if (!await context.ChiTietDonHangs.AnyAsync())
            {
                await context.ChiTietDonHangs.AddRangeAsync(chiTietDonHangs);
            }

            // Lưu vào SQL Server
            await context.SaveChangesAsync();
        }
    }
}