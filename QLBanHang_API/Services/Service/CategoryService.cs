using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PBL6_QLBH.Models;
using QLBanHang_API.Dto;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Request;
using System.Reflection.Metadata.Ecma335;

namespace QLBanHang_API.Service
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;
		public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
		}
		public async Task<List<CategoryDto>> GetAllCategoriesAsync()
		{
			var categories = await _categoryRepository.GetAllCategoriesAsync();
			return _mapper.Map<List<CategoryDto>>(categories);
		}
		public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
		{
			var category = await _categoryRepository.GetCategoryByIdAsync(id);
			return _mapper.Map<CategoryDto>(category);
		}
		public async Task<bool> CreateCategoryAsync(CategoryDto model)
		{
			var category = _mapper.Map<Category>(model);
			return await _categoryRepository.CreateCategoryAsync(category);
		}
		public async Task<bool> UpdateCategoryAsync(CategoryDto model)
		{
			var category = _mapper.Map<Category>(model);
			return await _categoryRepository.UpdateCategoryAsync(category);
		}
		public async Task<bool> DeleteCategoryAsync(Guid categoryId)
		{
			return await _categoryRepository.DeleteCategoryAsync(categoryId);
		}
		public async Task<List<CategoryDetailDto>> GetAllDetailCategory()

		{
			var categories = await _categoryRepository.GetAllDetailCategory();
            return _mapper.Map<List<CategoryDetailDto>>(categories);
		}

        public async Task<(List<CategoryDetailDto> categories, int totalRecords)> GetFilteredCategoriesAsync(int page, int pageSize, string searchQuery, string sortCriteria, bool isDescending)
        {
            // Lấy query đã lọc từ repository
            var query = _categoryRepository.GetFilteredCategoriesQuery(searchQuery, sortCriteria, isDescending);

            // Đếm tổng số danh mục sau khi lọc
            var totalRecords = await query.CountAsync();

            // Áp dụng phân trang
            var pagedCategories = await query.Skip((page - 1) * pageSize)
                                              .Take(pageSize)
                                              .ProjectTo<CategoryDetailDto>(_mapper.ConfigurationProvider)  // Ánh xạ trực tiếp từ query sang CategoryDetailDto
                                              .ToListAsync();

            return (pagedCategories, totalRecords);
        }

        // Lấy tổng số danh mục (dùng để tính tổng số trang)
        public async Task<int> GetTotalCategoriesAsync(string searchQuery)
        {
            var query = _categoryRepository.GetFilteredCategoriesQuery(searchQuery, "name", false);
            return await query.CountAsync();
        }

		public async Task<List<ProductDto>> GetProductsByCategoryIdAsync(Guid categoryId)
		{
			var products = await _categoryRepository.GetProductByCategoryIdAsync(categoryId);

			return _mapper.Map<List<ProductDto>>(products);
		}

		public async Task<bool> IsCategoryNameExistsAsync(string categoryName)
		{
			return await _categoryRepository.IsCategoryNameExistsAsync(categoryName);
		}

	}
}
