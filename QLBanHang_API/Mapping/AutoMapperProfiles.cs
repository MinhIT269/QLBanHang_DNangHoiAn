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
            //Product
            CreateMap<ProductDto, Product> ().ReverseMap();
            CreateMap<CategoryDto,Category> ().ReverseMap();
            CreateMap<ProductImageDto, ProductImage>().ReverseMap();
            //Brand
            CreateMap<Brand,BrandDto>().ReverseMap();
            CreateMap<Brand, UpBrandDto>().ReverseMap();
            CreateMap<Brand, AddBrandDto>().ReverseMap();
            //Location
            CreateMap<Location,LocationDto>().ReverseMap();
            CreateMap<Location,UpLocationDto>().ReverseMap();
            CreateMap<Location, AddLocationDto>().ReverseMap();
            //Order
            CreateMap<Order,OrderDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
            //Promotion
            CreateMap<Promotion,PromotionDto>().ReverseMap();
            CreateMap<Promotion, UpPromotionDto>().ReverseMap();
            CreateMap<Promotion, AddPromotionDto>().ReverseMap();
            //Review
            CreateMap<Review,ReviewDto>().ReverseMap();
            CreateMap<Review, UpReviewDto>().ReverseMap();
            CreateMap<Review, AddReviewDto>().ReverseMap();
            //User UserInfo
            CreateMap<UserInfo, UserInfoDto>().ReverseMap();
            CreateMap<UserInfo, AddUserInfoDto>().ReverseMap();
            CreateMap<User, AddUserDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();

        }
    }
}
