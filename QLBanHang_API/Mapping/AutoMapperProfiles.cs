using AutoMapper;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Request;

namespace QLBanHang_API.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //Product
            CreateMap<ProductRequest, Product>()
                .ForMember(dest => dest.ProductCategories, opt => opt.MapFrom(src =>
                           src.CategoryIds!.Select(id => new ProductCategory
                           {
                               CategoryId = id
                           })))
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom((src, dest) =>
                            src.AdditionalImageUrls?.Select(url => new ProductImage
                            {
                                ProductImageId = Guid.NewGuid(),
                                ImageUrl = url
                            }).ToList() ?? new List<ProductImage>()));
            CreateMap<Product, ProductDto>()
                 .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand!.BrandName))
                 .ForMember(dest => dest.CategoryNames, opt => opt.MapFrom(src => src.ProductCategories!.Select(pc => pc.Category.CategoryName))) // danh sach category
                 .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages!.Select(pi => pi.ImageUrl))); // danh sach anh phu
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<ProductImageDto, ProductImage>().ReverseMap();
            //Brand
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Brand, UpBrandDto>().ReverseMap();
            CreateMap<Brand, AddBrandDto>().ReverseMap();
            //Location
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<Location, UpLocationDto>().ReverseMap();
            CreateMap<Location, AddLocationDto>().ReverseMap();
            //Order
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
            //Promotion
            CreateMap<Promotion, PromotionDto>().ReverseMap();
            CreateMap<Promotion, UpPromotionDto>().ReverseMap();
            CreateMap<Promotion, AddPromotionDto>().ReverseMap();
            //Review
            CreateMap<Review, ReviewDto>().ReverseMap();
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
