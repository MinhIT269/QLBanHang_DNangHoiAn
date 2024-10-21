using AutoMapper;
using PBL6_QLBH.Models;
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
        }
    }
}
