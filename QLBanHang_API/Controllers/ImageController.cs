using Microsoft.AspNetCore.Mvc;
using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private IImageService _imageService;
        public ImageController(IImageService image)
        {
            _imageService = image;
        }

        [HttpPost("upload-temp")]
        public async Task<IActionResult> UploadImages(IFormFile files)
        {
            if (files == null)
            {
                return BadRequest("No files were uploaded.");
            }

            var uploadedImageUrls = await _imageService.UploadImageTempAsync(files, HttpContext);
            return Ok(new { urls = uploadedImageUrls }); // Trả về danh sách đường dẫn hình ảnh dưới dạng JSON
        }
    }
}
