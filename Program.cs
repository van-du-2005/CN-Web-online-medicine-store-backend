using OnlineMedicineStoreBackend.Data;
using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Services;      // Khai báo nơi chứa Đầu bếp
using OnlineMedicineStoreBackend.Repositories;  // Khai báo nơi chứa Kho dữ liệu

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddDbContext<OnlineMedicineStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Bơm nguyên liệu cho Controller hoạt động
builder.Services.AddControllers();

// === ĐĂNG KÝ KẾT NỐI (DEPENDENCY INJECTION) ===
// Bước cực kỳ quan trọng để ThuocController có thể gọi được ProductService
builder.Services.AddScoped<IThuocRepository, ThuocRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// === ĐĂNG KÝ KẾT NỐI (DEPENDENCY INJECTION) ===
builder.Services.AddScoped<IThuocRepository, ThuocRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// THÊM 2 DÒNG NÀY VÀO:
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// ---SEED DATA ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<OnlineMedicineStoreDbContext>();
        // Chạy Seed Data (bên trong hàm này đã có lệnh MigrateAsync tự tạo DB)
        await DbInitializer.SeedDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Có lỗi xảy ra khi khởi tạo Database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Chỉ đường cho các API
app.MapControllers();

// Chỉ được gọi 1 lần duy nhất ở cuối file
app.Run();