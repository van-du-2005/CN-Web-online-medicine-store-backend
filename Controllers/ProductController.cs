using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.Services;

namespace OnlineMedicineStoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public ProductController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 12)
        {
            var products = await _productService.GetAvailableProductsAsync(page, pageSize);
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var product = await _productService.GetProductDetailAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm." });
            }
            return Ok(product);
        }

        // GET: api/Product/search
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
        {
            var products = await _productService.SearchProductsAsync(searchTerm, page, pageSize);
            return Ok(products);
        }
    }
}
