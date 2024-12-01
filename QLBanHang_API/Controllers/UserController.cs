using Microsoft.AspNetCore.Mvc;
using QLBanHang_API.Services.IService;
using QLBanHang_API.Services.Service;

namespace QLBanHang_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;
        public UserController(IUserService user)
        {
            _user = user;
        }
        [HttpGet("GetFilteredUsers")]
        public async Task<IActionResult> GetFilteredUsers([FromQuery] string searchQuery = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 8, [FromQuery] string sortCriteria = "name", [FromQuery] bool isDescending = false)
        {
            var users = await _user.GetFilteredUsers(page, pageSize, searchQuery, sortCriteria, isDescending);
            return Ok(users);
        }
    }
}
