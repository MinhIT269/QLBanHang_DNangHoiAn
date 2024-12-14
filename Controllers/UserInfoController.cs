using Microsoft.AspNetCore.Mvc;
using PBL6.Dto;
using PBL6.Services.IService;
using QLBanHang_API.Dto;

namespace PBL6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfoService userInfoService;
        public UserInfoController(IUserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }

        //Lay UserInfo by Id
        // api/UserInfo/Get/username=?
        [HttpGet]
        [Route("Get/{username}")]
        public async Task<IActionResult> GetUserInfo(string username)
        {
            var userInfo = await userInfoService.GetByUserName(username);
      

            if (userInfo == null)
            {
                return NotFound();
            }
            return Ok(userInfo);
        }

        //Update UserInfo
        // /api/UserInfo/Update/username=?
        [HttpPut]
        [Route("Update/{username}")]
        public async Task<IActionResult> UpdateUserInfo([FromRoute] string username, UpdateUserInfoDto userInfoUpdate)
        {
            var userInfo = await userInfoService.UpdateUserInfo(username, userInfoUpdate);
            if (userInfo == null)
            {
                return NotFound();
            }
            return Ok(userInfo);
        }

        //Add UserInfo
        // /api/UserInfo/Add/username=?
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddUserInfo([FromBody] AddUserInfoDto userInfoDto)
        {
            var userInfo = await userInfoService.AddUserInfo(userInfoDto);
            if (userInfo == null)
            {
                return BadRequest();
            }
            return Ok(userInfo);
        }
    }
}
