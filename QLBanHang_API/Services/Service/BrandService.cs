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

		// Get All Brands
		public async Task<List<BrandDto>> GetAllBrands()
		{
			var brands = await brandRepository.GetAllBrandAsync();
			return mapper.Map<List<BrandDto>>(brands);
		}
        //Get Brand by Name
        public async Task<BrandDto> GetBrandByName(string brandName)
		{
			var brand = await brandRepository.GetBrandByNameAsync(brandName);
			var brandDto = mapper.Map<BrandDto>(brand);
            return brandDto;
		}

		//Update Brand
		public async Task<BrandDto> UpdateBrand(UpBrandDto brandUpdate)
		{
			var brand = mapper.Map<Brand>(brandUpdate);
			var brandDomain = await brandRepository.UpdateBrandAsync(brand);
			var brandDto = mapper.Map<BrandDto>(brand);
			return brandDto;
		}

		//Add Brand 
		public async Task<BrandDto> AddBrand(AddBrandDto brandAdd)
		{
			var brand = mapper.Map<Brand> (brandAdd);
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
