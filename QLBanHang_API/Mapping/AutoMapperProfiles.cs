using AutoMapper;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Request;
using System.Reflection;

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
			CreateMap<Order, OrderDto>()
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!.UserName))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User!.Email))
				.ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Transaction!.PaymentMethod!.Name))
				.ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Promotion!.Code))
                .ForMember(dest => dest.totalProduct, opt => opt.MapFrom(src => src.OrderDetails!.Select(od => od.ProductId).Distinct().Count()));

			CreateMap<Order, OrderDetailDto>()
				.ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.OrderDetails!.Select(od => new ProductDetailOrder
				{
                    PromotionPrice = od.Product.PromotionPrice,
					ImageUrl = od.Product.ImageUrl,
				    Warranty = od.Product.Warranty,
                    Name = od.Product.Name,
					Price = od.UnitPrice,
					Quantity = od.Quantity
				})))
				.ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => new UserInfoDto
				{
                    FirstName = src.User!.UserInfo!.FirstName,
                    LastName = src.User.UserInfo.LastName,
					Email = src.User.Email!,
					PhoneNumber = src.User.UserInfo.PhoneNumber,
					Address = src.User.UserInfo.Address,
					UserName = src.User.UserName!,
                    Gender = src.User.UserInfo.Gender,
                }))
				;


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
            //Cart Item
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
			CreateMap<CartItem, CartItemRequest>()
			.ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId))
			.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
			.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.ProductId))
			.ForMember(dest => dest.CartItemId, opt => opt.MapFrom(src => src.CartItemId));
			CreateMap<OrderDetail,OrderDetailsRequest>().ReverseMap();
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
			CreateMap<User, UserAdminDto> ()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.UserInfo!.PhoneNumber))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Orders!.Count()))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Orders!.Sum(order => order.TotalAmount)));

        }
	}
}
