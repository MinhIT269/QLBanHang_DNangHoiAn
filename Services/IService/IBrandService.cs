using Microsoft.AspNetCore.Http.HttpResults;
using PBL6.Dto;
using PBL6.Dto.Request;

namespace PBL6.Services.IService
{
    public interface IBrandService
    {

        Task<List<BrandDto>> GetAllBrands();
        Task<BrandDto> GetBrandByName(string brandName);
        Task<BrandRequest> GetBrandByIdAsync(Guid id);
        Task<BrandDto> UpdateBrand(Guid id, UpBrandDto brandUpdate);
        Task<BrandDto> DeleteBrand(Guid id);
        Task<BrandDto> AddBrand(BrandRequest brand);
        Task<(List<BrandDetailDto> brands, int totalRecods)> GetFilteredCategoriesAsync(int page, int pageSize, string searchQuery, string sortCriteria, bool isDescending);
        Task<int> GetTotalBrandsAsync(string searchQuery);
        Task<bool> IsBrandNameExists(string brandName);
        Task<bool> HasProductsByBrandIdAsync(Guid brandId);
    }
}
