namespace OnlineMedicineStoreBackend.Services.auth
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}