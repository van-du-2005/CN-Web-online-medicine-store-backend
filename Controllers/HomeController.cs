using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineMedicineStoreBackend.Models;
using OnlineMedicineStoreBackend.Services;

namespace OnlineMedicineStoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productList = await _productService.GetAvailableProductsAsync(1, 8);
            return Ok(productList.Products);
        }

        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return Ok(new { Message = "Privacy Policy" });
        }

        [HttpGet("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return Ok(new { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
