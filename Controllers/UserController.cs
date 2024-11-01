using Microsoft.AspNetCore.Mvc;
using PBL6.Dto;
using PBL6.Services.IService;
using PBL6_QLBH.Models;

namespace PBL6.Controllers
{
    [ApiController]
    [Route("[controller]")]
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



    }
}
