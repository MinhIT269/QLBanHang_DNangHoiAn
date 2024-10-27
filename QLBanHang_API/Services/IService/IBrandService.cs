using PBL6_QLBH.Models;
using QLBanHang_API.Dto;

namespace QLBanHang_API.Service
{
	public interface IBrandService
	{
		Task<BrandDto> GetBrandByName(string brandName);
		Task<BrandDto> UpdateBrand(Guid id, UpBrandDto brandUpdate);
		Task<BrandDto> DeleteBrand(Guid id);
		Task<BrandDto> AddBrand(AddBrandDto brand);
	}
}
