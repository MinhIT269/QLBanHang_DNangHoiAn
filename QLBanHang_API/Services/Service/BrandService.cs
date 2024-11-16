using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Dto.Request;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Repositories.Repository;
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
		public async Task<BrandRequest> GetBrandByIdAsync(Guid id)
		{
			var brand = await brandRepository.GetBrandByIdAsync(id);
			var brandDto = mapper.Map<BrandRequest>(brand);
			return brandDto;
		}

		//Add Brand 
		public async Task<BrandDto> AddBrand(BrandRequest brandAdd)
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

		public async Task<(List<BrandDetailDto> brands, int totalRecods)> GetFilteredCategoriesAsync(int page, int pageSize, string searchQuery, string sortCriteria, bool isDescending)
		{
			var query = brandRepository.GetFilteredBrandsQuery(searchQuery, sortCriteria, isDescending);

			var totalRecords = await query.CountAsync();

			var pagedBrands = await query.Skip((page-1) * pageSize)
				                         .Take(pageSize)
										 .ProjectTo<BrandDetailDto>(mapper.ConfigurationProvider)
										 .ToListAsync();
			return (pagedBrands, totalRecords);
		}

		public async Task<bool> HasProductsByBrandIdAsync(Guid brandId)
		{
			return await brandRepository.HasProductsByBrandIdAsync(brandId);
		}
		public async Task<int> GetTotalBrandsAsync(string searchQuery)
		{
			var query = brandRepository.GetFilteredBrandsQuery(searchQuery, "name", false);
			return await query.CountAsync();
		}

		public async Task<bool> IsBrandNameExists(string brandName)
		{
			return await brandRepository.IsBrandNameExists(brandName);
		}

		//Update Brand
		public async Task<BrandDto> UpdateBrand(Guid id, UpBrandDto brandUpdate)
		{
			var brand = mapper.Map<Brand>(brandUpdate);
			var brandDomain = await brandRepository.UpdateBrandAsync(id, brand);
			var brandDto = mapper.Map<BrandDto>(brand);
			return brandDto;
		}
	}
}
