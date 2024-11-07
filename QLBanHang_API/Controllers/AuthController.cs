using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Dto.Response;
using QLBanHang_API.Repositories.IRepository;
using System.Runtime.CompilerServices;

namespace QLBanHang_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<User> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequestDto)
        {
            var user = new User()
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.Email,
            };
            var result = await userManager.CreateAsync(user, registerRequestDto.Password);
            if (result.Succeeded)
            {
                if (string.IsNullOrEmpty(registerRequestDto.Role))
                {
                    registerRequestDto.Role = "User";
                    result = await userManager.AddToRoleAsync(user,registerRequestDto.Role);
                    if (result.Succeeded)
                    {
                        return Ok("Successfully Registered as a User");
                    }
                }
                else
                {
                    result = await userManager.AddToRoleAsync(user, registerRequestDto.Role);
                    if (result.Succeeded)
                    {
                        return Ok("Register Successful not a User");
                    }
                }
            }
            return BadRequest("UserName already exists");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await userManager.FindByNameAsync(loginRequest.UserName);
            if (user != null)
            {
                var identityResult = await userManager.CheckPasswordAsync(user,loginRequest.Password);
                if (identityResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    //CreateToken
                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponse()
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                }
            }
            return BadRequest("UserName or PassWord wrong");
        }
    }
}
