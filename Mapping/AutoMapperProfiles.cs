using AutoMapper;
using PBL6.Dto;
using PBL6_QLBH.Models;

namespace PBL6.Mapping
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<ReviewDto, Review>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
