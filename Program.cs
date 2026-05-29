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
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI.Microsoft;
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

// 1. KÍCH HOẠT TÍNH NĂNG ĐỌC CONTROLLER
builder.Services.AddControllers();

// 2. KÍCH HOẠT GIAO DIỆN SWAGGER ĐỂ TEST API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. KẾT NỐI DATABASE
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
builder.Services.AddScoped<IOrderRepositoryAdmin, OrderRepositoryAdmin>();
builder.Services.AddScoped<IOrderServiceAdmin, OrderServiceAdmin>();


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

// MIDDLEWARE BẮT LỖI TOÀN CỤC
app.UseMiddleware<GlobalExceptionMiddleware>();

// HIỂN THỊ GIAO DIỆN WEB SWAGGER 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ==========================================
// THỨ TỰ MIDDLEWARE CHUẨN CHỈNH
// ==========================================
app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();