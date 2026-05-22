using System;
using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Models;

namespace OnlineMedicineStoreBackend.Data
{
    public class OnlineMedicineStoreDbContext : DbContext
    {
        public OnlineMedicineStoreDbContext(DbContextOptions<OnlineMedicineStoreDbContext> options) : base(options)
        {
        }

        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<Thuoc> Thuocs { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public DbSet<LoThuoc> LoThuocs { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<DiaChiKhachHang> DiaChiKhachHangs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Bật Collation hỗ trợ chuẩn Tiếng Việt cho SQL Server
            modelBuilder.UseCollation("Vietnamese_CI_AS");

            // ==========================================
            // 1. CẤU HÌNH FLUENT API
            // ==========================================

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.Property(e => e.MaNguoiDung).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.NgayTao).HasDefaultValueSql("GETDATE()");
                entity.HasIndex(e => e.TenDangNhap).IsUnique().HasDatabaseName("uq_NguoiDung_tendangnhap");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.Property(e => e.MaKhachHang).HasDefaultValueSql("NEWID()");
                entity.HasIndex(e => e.MaNguoiDung).IsUnique().HasDatabaseName("uq_KhachHang_nguoidung");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.Property(e => e.MaNhanVien).HasDefaultValueSql("NEWID()");
                entity.HasIndex(e => e.MaNguoiDung).IsUnique().HasDatabaseName("uq_NhanVien_nguoidung");
            });

            modelBuilder.Entity<GioHang>(entity =>
            {
                entity.Property(e => e.MaGioHang).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.NgayTao).HasDefaultValueSql("GETDATE()");
                entity.HasIndex(e => e.MaKhachHang).IsUnique().HasDatabaseName("uq_GioHang_khachhang");
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.Property(e => e.MaDonHang).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.NgayDat).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.Property(e => e.MaChiTiet).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.ThanhTien).HasComputedColumnSql("([so_luong]*[don_gia])", stored: true);
            });

            modelBuilder.Entity<ChiTietGioHang>(entity =>
            {
                entity.Property(e => e.MaChiTiet).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.NgayThem).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.SoLuong).HasDefaultValue(1);
            });

            modelBuilder.Entity<DanhMuc>().Property(e => e.MaDanhMuc).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Thuoc>().Property(e => e.MaThuoc).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<LoThuoc>().Property(e => e.MaLo).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<PhieuNhap>().Property(e => e.MaPhieuNhap).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<NhaCungCap>().Property(e => e.MaNhaCungCap).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<DiaChiKhachHang>().Property(e => e.MaDiaChi).HasDefaultValueSql("NEWID()");

            // ==========================================
            // 2. CẤU HÌNH DELETE BEHAVIOR (Tắt Cascade Delete)
            // ==========================================

            modelBuilder.Entity<DonHang>().HasOne(d => d.KhachHang).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKhachHang).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DonHang>().HasOne(d => d.DiaChiKhachHang).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaDiaChi).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoThuoc>().HasOne(lt => lt.PhieuNhap).WithMany(p => p.LoThuocs)
                .HasForeignKey(lt => lt.MaPhieuNhap).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoThuoc>().HasOne(lt => lt.NhaCungCap).WithMany(p => p.LoThuocs)
                .HasForeignKey(lt => lt.MaNhaCungCap).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChiTietDonHang>().HasOne(c => c.Thuoc).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(c => c.MaThuoc).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChiTietDonHang>().HasOne(c => c.LoThuoc).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(c => c.MaLo).OnDelete(DeleteBehavior.Restrict);   
        }
    }
}
