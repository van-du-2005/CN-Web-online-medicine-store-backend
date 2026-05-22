using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace OnlineMedicineStoreBackend.Services.auth
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailSettings = _config.GetSection("EmailSettings");

            var smtpClient = new SmtpClient(emailSettings["SmtpServer"])
            {
                Port = int.Parse(emailSettings["SmtpPort"]!),
                Credentials = new NetworkCredential(emailSettings["SenderEmail"], emailSettings["SenderPassword"]),
                EnableSsl = true,
            };

            // Bổ sung HTML để Email trông chuyên nghiệp hơn (Yêu cầu bổ sung UX/Bảo mật)
            string htmlBody = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f8fafc; border-radius: 10px;'>
                    <h2 style='color: #059669;'>Nhà Thuốc Sống Khỏe</h2>
                    <p>Xin chào,</p>
                    <p>{body}</p>
                    <p><i>Lưu ý: Tuyệt đối không chia sẻ mã này cho bất kỳ ai.</i></p>
                    <br/>
                    <p>Trân trọng,<br/>Đội ngũ CSKH</p>
                </div>";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings["SenderEmail"]!, emailSettings["SenderName"]),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true, // Cho phép hiển thị HTML
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}