using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QLBanHang_API.CustomActionFilters;
using QLBanHang_API.Request;
using QLBanHang_API.Service;

namespace QLBanHang_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet("GetAllCategories")]
		public async Task<IActionResult> GetAllCategories()
		{
			var categories = await _categoryService.GetAllCategoriesAsync();
			return Ok(categories);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCategory(Guid id)
		{
			var category = await _categoryService.GetCategoryByIdAsync(id);
			return Ok(category);
		}

        [HttpPost("Create")]
        [ValidateModel]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto category)
        {
            var success = await _categoryService.CreateCategoryAsync(category);
            if (!success)
            {
                return BadRequest("Unable to create category."); 
            }
            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
		[ValidateModel]
		public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryDto category)
		{
			var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
			if (existingCategory == null)
			{
				return NotFound();
			}
			category.CategoryId = id; // Ensure the correct ID is used
			var success = await _categoryService.UpdateCategoryAsync(category);
            if (!success)
            {
                return BadRequest("Unable to update category."); // Trả về thông báo nếu không thành công
            }

            return Ok("Thanh cong");
        }

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategory(Guid id)
		{
			var category = await _categoryService.GetCategoryByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}

            var success = await _categoryService.DeleteCategoryAsync(id);
            if (!success)
            {
                return BadRequest("Unable to delete category."); // Trả về thông báo nếu không thành công
            }

            return Ok("Thanh cong");
        }
	}
}
