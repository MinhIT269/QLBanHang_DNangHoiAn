using QLBanHang_API.Dto;

namespace QLBanHang_API.Service
{
	public interface IBrandService
	{
		Task<List<BrandDto>> GetAllBrands();
		Task<BrandDto> GetBrandByName(string brandName);
		Task<BrandDto> UpdateBrand(UpBrandDto brandUpdate);
		Task<BrandDto> DeleteBrand(Guid id);
		Task<BrandDto> AddBrand(AddBrandDto brand);
	}
}
