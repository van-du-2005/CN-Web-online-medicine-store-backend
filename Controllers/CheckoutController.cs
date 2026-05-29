using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.DTOs;
using OnlineMedicineStoreBackend.Services;
using System.Security.Claims;

namespace OnlineMedicineStoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IZaloPayService _payService;

        // Bỏ IAuthService vì .NET Core sẽ tự động phân giải token trong tầng Middleware HttpContext
        public CheckoutController(IOrderService orderService, IZaloPayService payService)
        {
            _orderService = orderService;
            _payService = payService;
        }

        // Tiện ích lấy MaKhachHang (UserId) trực tiếp từ Claims của Token Bearer đã được xác thực
        private Guid GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("id");
            return Guid.TryParse(userIdStr, out Guid userId) ? userId : Guid.Empty;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessCheckout([FromBody] CheckoutDTO checkoutDTO)
        {
            // 1. Kiểm tra DTO gửi từ Angular lên có bị null không
            if (checkoutDTO == null)
            {
                return BadRequest(new { success = false, message = "Dữ liệu yêu cầu thanh toán không hợp lệ." });
            }

            // 2. Dọn dẹp xác thực ModelState các trường không do Frontend nhập
            ModelState.Remove("MaKhachHang");
            foreach (var key in ModelState.Keys.Where(k => k.StartsWith("SanPhamDaMua")).ToList())
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                // Trả về danh sách lỗi cụ thể cho Angular hiển thị lên form
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { success = false, message = "Thông tin nhập vào không hợp lệ.", errors });
            }

            // 3. Kiểm tra tính hợp lệ của danh sách sản phẩm đặt mua
            if (checkoutDTO.SanPhamDaMua == null || !checkoutDTO.SanPhamDaMua.Any())
            {
                return BadRequest(new { success = false, message = "Giỏ hàng thanh toán không được để trống." });
            }

            // 4. Lấy mã khách hàng từ Token
            var maKhachHang = GetCurrentUserId();
            if (maKhachHang == Guid.Empty)
            {
                return Unauthorized(new { success = false, message = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại." });
            }

            try
            {
                checkoutDTO.MaKhachHang = maKhachHang;
                var donHang = await _orderService.CreateOrderAsync(checkoutDTO);

                // THƯỜNG GẶP: So khớp chuỗi từ Frontend gửi lên thường là chữ thường (zalopay, cod)
                string phuongThuc = checkoutDTO.PhuongThucThanhToan.ToLower();

                if (phuongThuc == "cod")
                {
                    // Trả trạng thái thành công ngay để Angular chuyển hướng sang trang lịch sử đơn hàng
                    return Ok(new { 
                        success = true, 
                        message = "Đặt hàng thành công với hình thức COD.",
                        paymentUrl = (string)null 
                    });
                }
                else if (phuongThuc == "zalopay")
                {
                    // Tạo cổng thanh toán trực tuyến và trả link về cho Angular tự window.location.href
                    string paymentUrl = await _payService.CreatePaymentAsync(donHang);
                    return Ok(new { 
                        success = true, 
                        message = "Khởi tạo cổng thanh toán ZaloPay thành công.",
                        paymentUrl = paymentUrl 
                    });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Phương thức thanh toán này hiện chưa được hệ thống hỗ trợ." });
                }
            }
            catch (Exception ex)
            {
                string detailError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new { success = false, message = "Lỗi xử lý hệ thống dữ liệu: " + detailError });
            }
        }

        // Endpoint nhận kết quả trả về (Return URL) sau khi khách hàng bấm nút quay lại từ Iframe/App ZaloPay
        [HttpGet("zalopay-return")]
        [AllowAnonymous] // Cho phép cổng ZaloPay hoặc Client redirect về không cần đính token
        public async Task<IActionResult> ZaloPayReturn([FromQuery] int status, [FromQuery] string apptransid)
        {
            if (string.IsNullOrEmpty(apptransid))
            {
                apptransid = Request.Query["apptransid"].ToString();
            }

            var zaloPayResult = _payService.ProcessZaloPayReturn(status, apptransid);

            if (zaloPayResult.IsSuccess && zaloPayResult.MaDonHang != Guid.Empty)
            {
                await _orderService.UpdatePaymentStatusAsync(zaloPayResult.MaDonHang, "DangXuLy");
                
                // Vì không phải MVC nên bạn không dùng RedirectToAction được, hãy chuyển hướng thẳng trình duyệt 
                // về link trang thành công của Angular
                return Redirect("http://localhost:4200/account/orders?status=success");
            }
            else
            {
                if (zaloPayResult.MaDonHang != Guid.Empty)
                {
                    await _orderService.UpdatePaymentStatusAsync(zaloPayResult.MaDonHang, "Thanh toán thất bại");
                }

                // Trả về trang giỏ hàng kèm tham số báo lỗi để hiển thị thông báo bên Angular
                return Redirect("http://localhost:4200/cart?status=failed");
            }
        }
    }
}