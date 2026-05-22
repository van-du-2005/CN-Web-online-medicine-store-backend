using OnlineMedicineStoreBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. KÍCH HOẠT TÍNH NĂNG ĐỌC CONTROLLER
builder.Services.AddControllers();

// 2. KÍCH HOẠT GIAO DIỆN SWAGGER ĐỂ TEST API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. KẾT NỐI DATABASE
builder.Services.AddDbContext<OnlineMedicineStoreCNWDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. CẤP VISA CHO FRONTEND (Chỉ cần 1 cục này thôi)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();
app.UseStaticFiles();
// ---SEED DATA ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<OnlineMedicineStoreCNWDbContext>();
        await DbInitializer.SeedDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Có lỗi xảy ra khi khởi tạo Database.");
    }
}

// HIỂN THỊ GIAO DIỆN WEB SWAGGER 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ==========================================
// THỨ TỰ MIDDLEWARE CHUẨN CHỈNH (Đừng đổi chỗ nha sếp)
// ==========================================
app.UseHttpsRedirection();

app.UseStaticFiles(); // 1. Mở cửa kho cho phép đọc file tĩnh (ảnh, css...)
app.UseRouting();     // 2. Bật định tuyến
app.UseCors("AllowAll"); // 3. Kiểm tra thẻ Visa (CORS)

app.MapControllers(); // 4. Vào Controller lấy dữ liệu

app.Run();