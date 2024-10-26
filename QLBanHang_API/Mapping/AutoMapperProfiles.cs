using AutoMapper;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Request;

namespace QLBanHang_API.Mapping
{
	public class AutoMapperProfiles : Profile
	{
        public AutoMapperProfiles()
        {
            CreateMap<ProductDto, Product> ().ReverseMap();
            CreateMap<CategoryDto,Category> ().ReverseMap();
            CreateMap<ProductImageDto, ProductImage>().ReverseMap();
            CreateMap<Brand,BrandDto>().ReverseMap();
            CreateMap<Location,LocationDto>().ReverseMap();
            CreateMap<Location,AddUpLocationDto>().ReverseMap();
            CreateMap<Order,OrderDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
            CreateMap<Promotion,PromotionDto>().ReverseMap();
            CreateMap<Review,ReviewDto>().ReverseMap();
            CreateMap<UserInfo, UserInfoDto>().ReverseMap();
            CreateMap<User, AddUserDto>().ReverseMap();
        }
    }
}
