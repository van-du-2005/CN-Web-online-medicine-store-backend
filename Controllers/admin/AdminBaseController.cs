using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineMedicineStoreBackend.Controllers.admin
{
    [ApiController]
    [Route("api/admin/[controller]")] 
    [Authorize(Roles = "Admin")] // Xác thực JWT "QuanLy,Admin,NhanVien"
    public abstract class AdminBaseController : ControllerBase
    {
    }
}