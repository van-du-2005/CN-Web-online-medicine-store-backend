using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OnlineMedicineStoreBackend.Models;
using System.Security.Cryptography;
using System.Text;

namespace OnlineMedicineStoreBackend.Services
{
    public class ZaloPayService : IZaloPayService
    {
        private readonly string AppId;
        private readonly string Key1;
        private readonly string Key2;
        private readonly string Endpoint;

        public ZaloPayService(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            
            AppId = configuration["ZaloPay:AppId"] ?? throw new InvalidOperationException("ZaloPay:AppId không được cấu hình");
            Key1 = configuration["ZaloPay:Key1"] ?? throw new InvalidOperationException("ZaloPay:Key1 không được cấu hình");
            Key2 = configuration["ZaloPay:Key2"] ?? throw new InvalidOperationException("ZaloPay:Key2 không được cấu hình");
            Endpoint = configuration["ZaloPay:Endpoint"] ?? throw new InvalidOperationException("ZaloPay:Endpoint không được cấu hình");
        }

        public async Task<string> CreatePaymentAsync(DonHang donHang)
        {
            var embeData = new { redirecturl = "http://localhost:5237/api/checkout/zalopay-return" };
            var item = new[]
            {
                new
                {
                    itemname = "Đơn hàng nhà thuốc",
                    itemprice = donHang.ThanhToan,
                    itemquantity = 1
                }
            };

            var utcNow = DateTime.UtcNow;
            var tzVietnam = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, tzVietnam);

            // Nhúng MaDonHang trực tiếp vào app_trans_id (39 ký tự, đúng giới hạn ZaloPay)
            var appTransId = now.ToString("yyMMdd") + "_" + donHang.MaDonHang.ToString("N");

            var param = new Dictionary<string, string>
            {
                { "app_id",       AppId },
                { "app_user",     "KhachHang_" + donHang.MaKhachHang },
                { "app_time",     DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString() },
                { "amount",       donHang.ThanhToan.ToString("0") },
                { "app_trans_id", appTransId },
                { "embed_data",   JsonConvert.SerializeObject(embeData) },
                { "item",         JsonConvert.SerializeObject(item) },
                { "description",  "Thanh toán đơn hàng #" + donHang.MaDonHang },
                { "bank_code",    "zalopayapp" }
            };

            string dataToHash = AppId + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" +
                                 param["amount"] + "|" + param["app_time"] + "|" +
                                 param["embed_data"] + "|" + param["item"];

            param.Add("mac", ComputeHmacSha256(dataToHash, Key1));

            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(param);
            var response = await client.PostAsync(Endpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<dynamic>(responseString);

            if (responseData != null && responseData.return_code == 1)
            {
                return responseData.order_url;
            }
            else
            {
                throw new Exception("Lỗi khi tạo đơn hàng ZaloPay: " + responseData?.return_message);
            }
        }

        public (bool IsSuccess, Guid MaDonHang) ProcessZaloPayReturn(int status, string transactionId)
        {
            if (status != 1 || string.IsNullOrEmpty(transactionId))
                return (false, Guid.Empty);

            // Parse MaDonHang từ app_trans_id
            var underscoreIndex = transactionId.IndexOf('_');
            if (underscoreIndex >= 0)
            {
                var guidPart = transactionId.Substring(underscoreIndex + 1);
                if (Guid.TryParse(guidPart, out Guid maDonHang))
                    return (true, maDonHang);
            }

            return (false, Guid.Empty);
        }

        private string ComputeHmacSha256(string message, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using var hmacsha256 = new HMACSHA256(keyBytes);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
        }
    }
}