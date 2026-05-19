using System;

namespace OnlineMedicineStoreBackend.DTOs
{
    // DTO dùng cho API [HttpPost("add")]
    public class AddToCartRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    // DTO dùng cho API [HttpPut("update-quantity")]
    public class UpdateQuantityRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}