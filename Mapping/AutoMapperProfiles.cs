using AutoMapper;
using PBL6.Dto;
using PBL6.Dto.Request;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Request;

namespace PBL6.Mapping
{
    public class AutoMapperProfiles:Profile
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
                 .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages!.Select(pi => pi.ImageUrl))); // danh sach anh 
            
            //fix error Mapping
            CreateMap<ProductImage, string>().ConvertUsing(pi => pi.ImageUrl);

            //Category
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<Category, CategoryDetailDto>()
                 .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src =>
                     src.ProductCategories!.Any() ?
                     src.ProductCategories!.Select(pc => pc.Product).Where(p => p != null).Min(p => p.Price) : 0))
                 .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src =>
                     src.ProductCategories!.Any() ?
                     src.ProductCategories!.Select(pc => pc.Product).Where(p => p != null).Max(p => p.Price) : 0))
                 .ForMember(dest => dest.ProductStock, opt => opt.MapFrom(src =>
                     src.ProductCategories!.Any() ?
                     src.ProductCategories!.Select(pc => pc.Product).Where(p => p != null).Sum(p => p.Stock) : 0));
            CreateMap<ProductImageDto, ProductImage>().ReverseMap();

            //Brand
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Brand, UpBrandDto>().ReverseMap();
            CreateMap<Brand, BrandRequest>().ReverseMap();
            CreateMap<Brand, BrandDetailDto>()
                  .ForMember(dest => dest.StockAvailable, opt => opt.MapFrom(src => src.Products!.Sum(p => p.Stock)))
                  .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Products!.Sum(p => p.Stock * p.Price)))
                  .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.Locations));

            //Location
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<Location, UpLocationDto>().ReverseMap();
            CreateMap<Location, LocationRequest>().ReverseMap();

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

            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<ReviewDto, Review>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<PromotionDto, Promotion>().ReverseMap();
            CreateMap<CartDto, Cart>().ReverseMap();
            CreateMap<CartItemDto, CartItem>().ReverseMap();
            CreateMap<VideoDto, Video>().ReverseMap();
            CreateMap<BrandDto, Brand>().ReverseMap();
            CreateMap<LocationDto, Location>().ReverseMap();
            CreateMap<ProductCategoryDto, ProductCategory>().ReverseMap();
            CreateMap<OrderDetailDto, OrderDetail>().ReverseMap();
            CreateMap<OrderDto, Order>().ReverseMap();



        }
    }
}
