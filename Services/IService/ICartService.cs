using PBL6.Dto;
using PBL6_QLBH.Models;

namespace PBL6.Services.IService
{
    public interface ICartService
    {
        Task<CartDto> AddProductToCart(Guid productId);

        Task<CartDto> GetCartOfUser(Guid userId);
    }
}
