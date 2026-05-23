using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.Services;

namespace OnlineMedicineStoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]              
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("ask")]         // ← Route: api/chatbot/ask
        public async Task<IActionResult> Ask([FromBody] ChatbotRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Prompt))
            {
                return BadRequest(new { success = false, message = "Câu hỏi không được để trống." });
            }

            var result = await _chatbotService.GetAiRecommendationAsync(request.Prompt);

            return Ok(new { success = true, data = result });
        }
    }

    // DTO nhận request từ Angular
    public class ChatbotRequest
    {
        public string Prompt { get; set; } = string.Empty;
    }
}