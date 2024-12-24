using Microsoft.AspNetCore.Mvc;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Service;
using QLBanHang_API.Services.IService;
using QLBanHang_API.Services.Service;
using System.Runtime.CompilerServices;

namespace QLBanHang_API.Controllers
{
    
    public class CartController : ControllerBase
	{
		private readonly ICartService cartService;
        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet]
		[Route("GetAllCartItem/{id:guid}")]
		public async Task<IActionResult> GetAllCartItems([FromRoute] Guid id)
		{
			var cartItems = await cartService.GetAllCartItems(id);
			if (cartItems == null)
			{
				return NotFound();
			}
			return Ok(cartItems);
		}

		[HttpPost]
		[Route("AddCartItem")]
		public async Task<IActionResult> AddCartItem([FromBody] CartItemRequest cartRequest)
		{
			var cartItems = await cartService.AddCartItem(cartRequest);
            if (cartItems ==null)
            {
				return BadRequest();
            }
			return Ok(cartItems);
        }

		[HttpDelete]
		[Route("DeleteCartItem")]
		public async Task<IActionResult> DeleteCartItem([FromBody] List<CartItemRequest> cartRequests)
		{
			var cartItems = await cartService.DeleteCartItem(cartRequests);
			if (cartItems == null )
			{
				return BadRequest();
			}
			return Ok();
		}
		[HttpPut]
		[Route("UpdateCart")]
		public async Task<IActionResult> UpdateCartItem([FromBody] List<CartItemRequest> cartItemRequests)
		{
            if (cartItemRequests == null || !cartItemRequests.Any())
            {
                return BadRequest("No cart items provided.");
            }
            var cartItem = await cartService.UpdateCartItem(cartItemRequests);
			if(cartItem == null)
			{
				return BadRequest();
			}
			return Ok(cartItem);
		}

        
    }
}                                                
