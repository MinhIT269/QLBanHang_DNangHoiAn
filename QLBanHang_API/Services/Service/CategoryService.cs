using AutoMapper;
using PBL6_QLBH.Models;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Request;

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
		public async Task CreateCategoryAsync(CategoryDto model)
		{
		 	var category = _mapper.Map<Category>(model);
			await _categoryRepository.CreateCategoryAsync(category);
		}
		public async Task UpdateCategoryAsync(CategoryDto model)
		{
			var category = _mapper.Map<Category>(model);
			await _categoryRepository.UpdateCategoryAsync(category);
		}
		public async Task DeleteCategoryAsync(Guid categoryId)
		{
			await _categoryRepository.DeleteCategoryAsync(categoryId);
		}

	}
}
