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
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI.Microsoft;

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

var aiConfig = builder.Configuration.GetSection("GoogleGeminiAI");
string apiKey = aiConfig["ApiKey"] ?? throw new Exception("Chưa tồn tại API key cho Google Gemini AI");
string model = aiConfig["Model"] ?? throw new Exception("Chưa tồn tại model cho Google Gemini AI");
builder.Services.AddChatClient(new GeminiChatClient(apiKey: apiKey, model: model));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddDbContext<OnlineMedicineStoreCNWDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------auth---------------
builder.Services.AddMemoryCache(); // Kích hoạt Cache
builder.Services.AddScoped<JwtUtils>();
builder.Services.AddScoped<IUserDRepository, UserDRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>(); 

// ---------------------khách hàng---------------

// account liên quan đến khách hàng 
builder.Services.AddScoped<IAccountDRepository, AccountDRepository>();
builder.Services.AddScoped<IAccountDService, AccountDService>();

// cart liên quan đến khách hàng
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IGioHangRepository, GioHangRepository>();
builder.Services.AddScoped<IDonHangRepository, DonHangRepository>();
builder.Services.AddScoped<IThuocRepository, ThuocRepository>();

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IZaloPayService, ZaloPayService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IChatbotService, ChatbotService>();

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
