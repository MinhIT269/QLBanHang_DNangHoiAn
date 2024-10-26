using AutoMapper;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Repositories.IRepository;
namespace QLBanHang_API.Service
{
    public class BrandService : IBrandService
	{
		private readonly IBrandRepository brandRepository;
		private readonly IMapper mapper;
		public BrandService(IBrandRepository brandRepository, IMapper mapper)
		{
			this.brandRepository = brandRepository;
			this.mapper = mapper;
		}

		//Get Brand by Name
		public async Task<BrandDto> GetBrandByName(string brandName)
		{
			var brand = await brandRepository.GetBrandByNameAsync(brandName);
			var brandDto = mapper.Map<BrandDto>(brand);
            return brandDto;
		}

		//Update Brand
		public async Task<BrandDto> UpdateBrand(Guid id, Brand brandUpdate)
		{
			var brand = await brandRepository.UpdateBrandAsync(id, brandUpdate);
			var brandDto = mapper.Map<BrandDto>(brand);
			return brandDto;
		}

		//Add Brand 
		public async Task<BrandDto> AddBrand(Brand brand)
		{
			var brandDomain = await brandRepository.AddBrandAsync(brand);
			var brandDto = mapper.Map<BrandDto>(brandDomain);
			return brandDto;
		}

		//Delete Brand
		public async Task<BrandDto> DeleteBrand(Guid id)
		{
			var brand = await brandRepository.DeleteBrandAsync(id);
			var brandDto = mapper.Map<BrandDto> (brand);
			return brandDto;
		}
	}
}
