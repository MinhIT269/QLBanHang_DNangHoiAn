using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang_API.Dto;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        //Get all User
        // /api/User/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userService.GetAllUsers();
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        //Add User 
        // /api/User/Add
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto userAdd)
        {
            var user = await userService.AddUser(userAdd);
            if (user == null)
            {
                return NotFound("Username already exists");
            }
            return Ok(user);
        }

        //Delete User

        [HttpDelete]
        [Route("Delete/{username}")]
        public async Task<IActionResult> DeleteUser([FromRoute]string username)
        {
            var user = await userService.DeleteUser(username);
            if (user == null)
            {
                return NotFound("User does not exist");
            }
            return Ok();
        }
    }
}
