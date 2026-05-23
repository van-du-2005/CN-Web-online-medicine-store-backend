using System.Text.Json;
using OnlineMedicineStoreBackend.Repositories;
using Microsoft.Extensions.AI;

namespace OnlineMedicineStoreBackend.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly IChatClient _chatClient;
        private readonly IUnitOfWork _unitOfWork;

        public ChatbotService(IChatClient chatClient, IUnitOfWork unitOfWork)
        {
            _chatClient = chatClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GetAiRecommendationAsync(string userPrompt)
        {
            var danhSachThuoc = await _unitOfWork.Thuocs.GetAllAsync();

            var thuocJson = JsonSerializer.Serialize(danhSachThuoc.Select(t => new
            {
                t.TenThuoc,
                t.TenKhoaHoc,
                t.DanhMuc,
                t.MoTa,
                t.GiaBan,
                t.HuongDanSuDung
            }));

            var messages = new List<ChatMessage> {
                new ChatMessage(ChatRole.System, $"Bạn là một trợ lý ảo giúp người dùng tìm kiếm thông tin về thuốc. Dưới đây là danh sách thuốc hiện có: {thuocJson}"),
                new ChatMessage(ChatRole.User, userPrompt)
            };

            var response = await _chatClient.GetResponseAsync(messages);
            return response.Text ?? response.ToString();
        }
    }
}