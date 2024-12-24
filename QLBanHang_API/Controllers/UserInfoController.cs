using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang_API.Dto;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [Route("Update/{id:guid}")]
        public async Task<IActionResult> UpdateUserInfo([FromRoute] Guid id, [FromBody]UpdateUserInfoDto userInfoUpdate)
        {
            var userInfo = await userInfoService.UpdateUserInfo(id, userInfoUpdate);
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
        public async Task<IActionResult> AddUserInfo([FromBody]AddUserInfoDto userInfoDto)
        {
            var userInfo = await userInfoService.AddUserInfo(userInfoDto);
            if (userInfo == null)
            {
                return BadRequest();
            }
            return Ok(userInfo);
        }

        [HttpGet]
        [Route("GetId/{id:guid}")]
        public async Task<IActionResult> GetUserInfoById(Guid id)
        {
            var userInfo = await userInfoService.GetUserById(id);
            if (userInfo == null)
            {
                return NotFound();
            }
            return Ok(userInfo);
        }
    }
}
