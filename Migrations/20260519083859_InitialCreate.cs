using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMedicineStoreBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhMucs",
                columns: table => new
                {
                    ma_danh_muc = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ten_danh_muc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thu_tu = table.Column<int>(type: "int", nullable: false),
                    danh_muc_cha_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    trang_thai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucs", x => x.ma_danh_muc);
                    table.ForeignKey(
                        name: "FK_DanhMucs_DanhMucs_danh_muc_cha_id",
                        column: x => x.danh_muc_cha_id,
                        principalTable: "DanhMucs",
                        principalColumn: "ma_danh_muc");
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    ma_nguoi_dung = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ten_dang_nhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    mat_khau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ho_ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    so_dien_thoai = table.Column<string>(type: "varchar(15)", nullable: true),
                    ngay_sinh = table.Column<DateOnly>(type: "date", nullable: true),
                    gioi_tinh = table.Column<bool>(type: "bit", nullable: true),
                    dia_chi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    vai_tro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    trang_thai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.ma_nguoi_dung);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCaps",
                columns: table => new
                {
                    ma_nha_cung_cap = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ten_cong_ty = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    nguoi_lien_he = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    so_dien_thoai = table.Column<string>(type: "varchar(20)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    dia_chi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trang_thai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCaps", x => x.ma_nha_cung_cap);
                });

            migrationBuilder.CreateTable(
                name: "Thuocs",
                columns: table => new
                {
                    ma_thuoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ma_danh_muc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ten_thuoc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ten_khoa_hoc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    loai_thuoc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    gia_ban = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    huong_dan_su_dung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ton_kho_hien_tai = table.Column<int>(type: "int", nullable: false),
                    hinh_anh = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    trang_thai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thuocs", x => x.ma_thuoc);
                    table.ForeignKey(
                        name: "FK_Thuocs_DanhMucs_ma_danh_muc",
                        column: x => x.ma_danh_muc,
                        principalTable: "DanhMucs",
                        principalColumn: "ma_danh_muc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KhachHangs",
                columns: table => new
                {
                    ma_khach_hang = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ma_nguoi_dung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    diem_tich_luy = table.Column<int>(type: "int", nullable: false),
                    hang_thanh_vien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHangs", x => x.ma_khach_hang);
                    table.ForeignKey(
                        name: "FK_KhachHangs_NguoiDungs_ma_nguoi_dung",
                        column: x => x.ma_nguoi_dung,
                        principalTable: "NguoiDungs",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    ma_nhan_vien = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ma_nguoi_dung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    chuc_vu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ngay_vao_lam = table.Column<DateOnly>(type: "date", nullable: true),
                    luong = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.ma_nhan_vien);
                    table.ForeignKey(
                        name: "FK_NhanViens_NguoiDungs_ma_nguoi_dung",
                        column: x => x.ma_nguoi_dung,
                        principalTable: "NguoiDungs",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiaChiKhachHangs",
                columns: table => new
                {
                    ma_dia_chi = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ma_khach_hang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ho_ten_nguoi_nhan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDT_nguoi_nhan = table.Column<string>(type: "varchar(15)", nullable: false),
                    dia_chi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    la_mac_dinh = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaChiKhachHangs", x => x.ma_dia_chi);
                    table.ForeignKey(
                        name: "FK_DiaChiKhachHangs_KhachHangs_ma_khach_hang",
                        column: x => x.ma_khach_hang,
                        principalTable: "KhachHangs",
                        principalColumn: "ma_khach_hang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GioHangs",
                columns: table => new
                {
                    ma_gio_hang = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ma_khach_hang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangs", x => x.ma_gio_hang);
                    table.ForeignKey(
                        name: "FK_GioHangs_KhachHangs_ma_khach_hang",
                        column: x => x.ma_khach_hang,
                        principalTable: "KhachHangs",
                        principalColumn: "ma_khach_hang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhieuNhaps",
                columns: table => new
                {
                    ma_phieu_nhap = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ma_nhan_vien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_nha_cung_cap = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ngay_nhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tong_tien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ghi_chu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trang_thai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuNhaps", x => x.ma_phieu_nhap);
                    table.ForeignKey(
                        name: "FK_PhieuNhaps_NhaCungCaps_ma_nha_cung_cap",
                        column: x => x.ma_nha_cung_cap,
                        principalTable: "NhaCungCaps",
                        principalColumn: "ma_nha_cung_cap",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhieuNhaps_NhanViens_ma_nhan_vien",
                        column: x => x.ma_nhan_vien,
                        principalTable: "NhanViens",
                        principalColumn: "ma_nhan_vien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    ma_don_hang = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ma_khach_hang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_dia_chi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ma_nhan_vien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ngay_dat = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    loai_don = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tam_tinh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    phi_van_chuyen = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    giam_gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    thanh_toan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    phuong_thuc_TT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trang_thai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.ma_don_hang);
                    table.ForeignKey(
                        name: "FK_DonHangs_DiaChiKhachHangs_ma_dia_chi",
                        column: x => x.ma_dia_chi,
                        principalTable: "DiaChiKhachHangs",
                        principalColumn: "ma_dia_chi",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonHangs_KhachHangs_ma_khach_hang",
                        column: x => x.ma_khach_hang,
                        principalTable: "KhachHangs",
                        principalColumn: "ma_khach_hang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonHangs_NhanViens_ma_nhan_vien",
                        column: x => x.ma_nhan_vien,
                        principalTable: "NhanViens",
                        principalColumn: "ma_nhan_vien");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietGioHangs",
                columns: table => new
                {
                    ma_chi_tiet = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ma_gio_hang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_thuoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    so_luong = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ngay_them = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietGioHangs", x => x.ma_chi_tiet);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHangs_GioHangs_ma_gio_hang",
                        column: x => x.ma_gio_hang,
                        principalTable: "GioHangs",
                        principalColumn: "ma_gio_hang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHangs_Thuocs_ma_thuoc",
                        column: x => x.ma_thuoc,
                        principalTable: "Thuocs",
                        principalColumn: "ma_thuoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoThuocs",
                columns: table => new
                {
                    ma_lo = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ma_thuoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_phieu_nhap = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_nha_cung_cap = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ngay_san_xuat = table.Column<DateOnly>(type: "date", nullable: true),
                    ngay_het_han = table.Column<DateOnly>(type: "date", nullable: false),
                    so_luong_nhap = table.Column<int>(type: "int", nullable: false),
                    so_luong_hien_tai = table.Column<int>(type: "int", nullable: false),
                    gia_nhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoThuocs", x => x.ma_lo);
                    table.ForeignKey(
                        name: "FK_LoThuocs_NhaCungCaps_ma_nha_cung_cap",
                        column: x => x.ma_nha_cung_cap,
                        principalTable: "NhaCungCaps",
                        principalColumn: "ma_nha_cung_cap",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoThuocs_PhieuNhaps_ma_phieu_nhap",
                        column: x => x.ma_phieu_nhap,
                        principalTable: "PhieuNhaps",
                        principalColumn: "ma_phieu_nhap",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoThuocs_Thuocs_ma_thuoc",
                        column: x => x.ma_thuoc,
                        principalTable: "Thuocs",
                        principalColumn: "ma_thuoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHangs",
                columns: table => new
                {
                    ma_chi_tiet = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ma_don_hang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_thuoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_lo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    so_luong = table.Column<int>(type: "int", nullable: false),
                    don_gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    thanh_tien = table.Column<decimal>(type: "decimal(18,2)", nullable: true, computedColumnSql: "([so_luong]*[don_gia])", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHangs", x => x.ma_chi_tiet);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_DonHangs_ma_don_hang",
                        column: x => x.ma_don_hang,
                        principalTable: "DonHangs",
                        principalColumn: "ma_don_hang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_LoThuocs_ma_lo",
                        column: x => x.ma_lo,
                        principalTable: "LoThuocs",
                        principalColumn: "ma_lo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_Thuocs_ma_thuoc",
                        column: x => x.ma_thuoc,
                        principalTable: "Thuocs",
                        principalColumn: "ma_thuoc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_ma_don_hang",
                table: "ChiTietDonHangs",
                column: "ma_don_hang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_ma_lo",
                table: "ChiTietDonHangs",
                column: "ma_lo");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_ma_thuoc",
                table: "ChiTietDonHangs",
                column: "ma_thuoc");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHangs_ma_gio_hang",
                table: "ChiTietGioHangs",
                column: "ma_gio_hang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHangs_ma_thuoc",
                table: "ChiTietGioHangs",
                column: "ma_thuoc");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucs_danh_muc_cha_id",
                table: "DanhMucs",
                column: "danh_muc_cha_id");

            migrationBuilder.CreateIndex(
                name: "IX_DiaChiKhachHangs_ma_khach_hang",
                table: "DiaChiKhachHangs",
                column: "ma_khach_hang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_ma_dia_chi",
                table: "DonHangs",
                column: "ma_dia_chi");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_ma_khach_hang",
                table: "DonHangs",
                column: "ma_khach_hang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_ma_nhan_vien",
                table: "DonHangs",
                column: "ma_nhan_vien");

            migrationBuilder.CreateIndex(
                name: "uq_GioHang_khachhang",
                table: "GioHangs",
                column: "ma_khach_hang",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_KhachHang_nguoidung",
                table: "KhachHangs",
                column: "ma_nguoi_dung",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoThuocs_ma_nha_cung_cap",
                table: "LoThuocs",
                column: "ma_nha_cung_cap");

            migrationBuilder.CreateIndex(
                name: "IX_LoThuocs_ma_phieu_nhap",
                table: "LoThuocs",
                column: "ma_phieu_nhap");

            migrationBuilder.CreateIndex(
                name: "IX_LoThuocs_ma_thuoc",
                table: "LoThuocs",
                column: "ma_thuoc");

            migrationBuilder.CreateIndex(
                name: "uq_NguoiDung_tendangnhap",
                table: "NguoiDungs",
                column: "ten_dang_nhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_NhanVien_nguoidung",
                table: "NhanViens",
                column: "ma_nguoi_dung",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhaps_ma_nha_cung_cap",
                table: "PhieuNhaps",
                column: "ma_nha_cung_cap");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhaps_ma_nhan_vien",
                table: "PhieuNhaps",
                column: "ma_nhan_vien");

            migrationBuilder.CreateIndex(
                name: "IX_Thuocs_ma_danh_muc",
                table: "Thuocs",
                column: "ma_danh_muc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHangs");

            migrationBuilder.DropTable(
                name: "ChiTietGioHangs");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "LoThuocs");

            migrationBuilder.DropTable(
                name: "GioHangs");

            migrationBuilder.DropTable(
                name: "DiaChiKhachHangs");

            migrationBuilder.DropTable(
                name: "PhieuNhaps");

            migrationBuilder.DropTable(
                name: "Thuocs");

            migrationBuilder.DropTable(
                name: "KhachHangs");

            migrationBuilder.DropTable(
                name: "NhaCungCaps");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "DanhMucs");

            migrationBuilder.DropTable(
                name: "NguoiDungs");
        }
    }
}
