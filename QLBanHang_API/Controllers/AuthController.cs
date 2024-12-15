using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PBL6.Dto.Response;
using PBL6.Repositories.IRepository;
using PBL6.Services.IService;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto.Request;
using System.Net.Mail;

namespace PBL6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly IEmailService emailService;
        public AuthController(UserManager<User> userManager, ITokenRepository tokenRepository, IEmailService emailService)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.emailService = emailService;
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
                    registerRequestDto.Role = "Customer";
                    result = await userManager.AddToRoleAsync(user, registerRequestDto.Role);
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

            var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
            return BadRequest($"Registration failed: {errorMessages}");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            User user = null;


            if (IsValidEmail(loginRequest.UserName))
            {
                user = await userManager.FindByEmailAsync(loginRequest.UserName);
            }
            else
            {
                user = await userManager.FindByNameAsync(loginRequest.UserName);
            }
            if (user != null)
            {
                var identityResult = await userManager.CheckPasswordAsync(user, loginRequest.Password);
                if (identityResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    Console.WriteLine("role size:" + roles[0].ToString());
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

        //Reset Password
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            var user = await userManager.FindByNameAsync(resetPasswordRequest.UserName);
            if (user == null)
            {
                return NotFound("Invalid UserName");
            }
            var result = await userManager.ResetPasswordAsync(user, resetPasswordRequest.Token, resetPasswordRequest.Password);
            if (result.Succeeded)
            {
                return Ok("Reset successful");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        //Send Mail
        [HttpPost]
        [Route("SendMail")]
        public async Task<IActionResult> SendEmail([FromBody] string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                return NotFound();
            }
            var result = await userManager.GeneratePasswordResetTokenAsync(user);
            var expirationDate = DateTime.Now.AddMinutes(15);
            var emailRequest = new EmailRequest()
            {
                EmailReceive = Email,
                Subject = "ResetPassWord",
                Body = "Copy this code to reset " + result
            };
            var resultEmail = await emailService.SendEmail(emailRequest);
            if (!resultEmail)
            {
                return BadRequest("Gửi Email không thành công");
            }
            return Ok("Đã gửi Email thành công");
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
