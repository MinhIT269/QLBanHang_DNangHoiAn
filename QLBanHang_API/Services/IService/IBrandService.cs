using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using System.Runtime.CompilerServices;

namespace QLBanHang_API.Service
{
	public interface IBrandService
	{
		Task<BrandDto> GetBrandByName(string brandName);
		Task<BrandDto> UpdateBrand(Guid id, Brand brandUpdate);
		Task<BrandDto> DeleteBrand(Guid id);
		Task<BrandDto> AddBrand(Brand brand);
	}
}
