using Microsoft.AspNetCore.Mvc;
using PBL6.CustomActionFilters;
using PBL6.Dto;
using PBL6.Services.IService;

namespace PBL6.Controllers
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

        [HttpGet("GetAllDetailCategory")]
        public async Task<IActionResult> GetAllDetailCategory()
        {
            var categories = await _categoryService.GetAllDetailCategory();
            return Ok(categories);
        }

        [HttpGet("GetFilteredCategories")]
        public async Task<IActionResult> GetFilteredCategories([FromQuery] string searchQuery = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 8, [FromQuery] string sortCriteria = "name", [FromQuery] bool isDescending = false)
        {
            var (categories, totalRecords) = await _categoryService.GetFilteredCategoriesAsync(page, pageSize, searchQuery, sortCriteria, isDescending);

            return Ok(categories);
        }

        [HttpGet("TotalPagesCategory")]
        public async Task<IActionResult> GetTotalPagesCategory([FromQuery] string searchQuery = "")
        {
            var totalRecords = await _categoryService.GetTotalCategoriesAsync(searchQuery);
            var totalPages = (int)Math.Ceiling((double)totalRecords / 8); // Điều chỉnh số item trên mỗi trang nếu cần
            return Ok(totalPages);
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
            if (await _categoryService.IsCategoryNameExistsAsync(category.CategoryName!))
            {
                return BadRequest("Tên danh mục đã tồn tại.");
            }

            var success = await _categoryService.CreateCategoryAsync(category);
            if (!success)
            {
                return BadRequest("Unable to create category.");
            }
            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto category)
        {
            if (await _categoryService.IsCategoryNameExistsAsync(category.CategoryName!))
            {
                return BadRequest("Tên danh mục đã tồn tại.");
            }
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
            var productsUsingCategory = await _categoryService.GetProductsByCategoryIdAsync(id);
            if (productsUsingCategory.Any())
            {
                return BadRequest("Danh mục này đang được sử dụng bởi các sản phẩm, không thể xóa!");
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
