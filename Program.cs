using OnlineMedicineStoreBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. KÍCH HOẠT TÍNH NĂNG ĐỌC CONTROLLER (Cực kỳ quan trọng)
builder.Services.AddControllers();

// 2. KÍCH HOẠT GIAO DIỆN SWAGGER ĐỂ TEST API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Kết nối Database
builder.Services.AddDbContext<OnlineMedicineStoreCNWDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

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

// 3. HIỂN THỊ GIAO DIỆN WEB SWAGGER (Chỉ hiện khi code ở máy cá nhân)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 4. ÁNH XẠ CÁC ĐƯỜNG DẪN API VÀO CONTROLLER
app.MapControllers();

app.Run();