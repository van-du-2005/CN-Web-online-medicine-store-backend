using OnlineMedicineStoreBackend.Data;
using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Middleware; 
using OnlineMedicineStoreBackend.Repositories; 
using OnlineMedicineStoreBackend.Services.auth;    
using OnlineMedicineStoreBackend.Services; 
using OnlineMedicineStoreBackend.utils;    
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text; 
using OnlineMedicineStoreBackend.Repositories.admin;
using OnlineMedicineStoreBackend.Services.admin;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization(); // Kích hoạt dịch vụ phân quyền

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddDbContext<OnlineMedicineStoreCNWDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------đăng kí services, repository auth---------------
builder.Services.AddMemoryCache(); // Kích hoạt Cache
builder.Services.AddScoped<JwtUtils>();
builder.Services.AddScoped<IUserDRepository, UserDRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>(); 

// ---------------------đăng kí services, repository khách hàng---------------
builder.Services.AddScoped<IAccountDRepository, AccountDRepository>();
builder.Services.AddScoped<IAccountDService, AccountDService>();

// ---------------------đăng kí services, repository admin---------------
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();


// cấu hình CORS để cho phép Frontend Angular truy cập API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // hỗ trợ Cookie bảo mật
    });
});

var app = builder.Build();

// ---SEED DATA ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<OnlineMedicineStoreCNWDbContext>();
        // Chạy Seed Data (bên trong hàm này đã có lệnh MigrateAsync tự tạo DB)
        await DbInitializer.SeedDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Có lỗi xảy ra khi khởi tạo Database.");
    }
}

// MIDDLEWARE BẮT LỖI TOÀN CỤC
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication(); // Bật xác thực trước khi phân quyền
app.UseAuthorization();  // Bật phân quyền sau khi đã xác thực

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
