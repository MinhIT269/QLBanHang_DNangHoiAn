using QLBanHang_API.Dto;

namespace QLBanHang_API.Service
{
	public interface IBrandService
	{
		Task<List<BrandDto>> GetAllBrandsAsync();
	}
}
