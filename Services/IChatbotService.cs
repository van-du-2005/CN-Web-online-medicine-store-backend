namespace OnlineMedicineStoreBackend.Services
{
    public interface IChatbotService
    {
        Task<string> GetAiRecommendationAsync(string userPrompt);
    }
}