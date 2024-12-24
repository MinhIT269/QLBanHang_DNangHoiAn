using Microsoft.AspNetCore.Mvc;
using PBL6.Dto;
using PBL6.Services.IService;
using PBL6_QLBH.Models;

namespace PBL6.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {

            _userService = userService;
        }


        [HttpPost("rate")]
        public async Task<ActionResult<ReviewDto>> createReview([FromBody] Review newReview) {
            return  Created(String.Empty, await _userService.createReview(newReview));
        }


        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserDetails(string username)
        {
            var user = await _userService.GetUser(username);

            return Ok(user);
        }

        [HttpPost("update-by-username/{username}")]
        public async Task<IActionResult> UpdateUserInfo(string username, [FromBody] UpdateUserDto updatedInfo)
        {
            var result = await _userService.UpdateUserInfoByUsernameAsync(username, updatedInfo);

            if (!result)
            {
                return NotFound(new { message = "User or UserInfo not found" });
            }

            return Ok(new { message = "UserInfo updated successfully" });
        }
        [HttpPost("update/{username}")]
        public async Task<IActionResult> UpdateUser([FromQuery] string username, [FromQuery] string password, [FromQuery] string email)
        {
            var result = await _userService.UpdateUserAsync(username, password, email);
            if (!result)
            {
                return NotFound(new { Message = "User not found" });
            }

            return Ok(new { Message = "User updated successfully" });
        }

        [HttpGet("GetFilteredUsers")]
        public async Task<IActionResult> GetFilteredUsers([FromQuery] string searchQuery = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 8, [FromQuery] string sortCriteria = "name", [FromQuery] bool isDescending = false)
        {
            var users = await _userService.GetFilteredUsers(page, pageSize, searchQuery, sortCriteria, isDescending);
            return Ok(users);
        }

        [HttpGet("TotalPagesUsers")]
        public async Task<IActionResult> GetTotalUsersPromotion([FromQuery] string searchQuery = "")
        {
            var totalRecords = await _userService.GetTotalUserAsync(searchQuery);
            var totalPages = (int)Math.Ceiling((double)totalRecords / 8); // Điều chỉnh số item trên mỗi trang nếu cần
            return Ok(totalPages);
        }

		[HttpGet("TotalUsers")]
		public async Task<IActionResult> TotalUsers([FromQuery] string searchQuery = "")
		{
			try
			{
				var totalRecords = await _userService.GetTotalUserAsync(searchQuery);
				return Ok(new { totalUsers = totalRecords });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
			}
		}


	}
}
