using QLBanHang_API.Services.IService;

namespace QLBanHang_API.Services.Service
{
	public class ImageService : IImageService
	{
		public async Task<string> UploadImageAsync(IFormFile image)
		{
			// Đường dẫn đến thư mục images
			var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "images");

			// Kiểm tra nếu thư mục không tồn tại thì tạo mới
			if (!Directory.Exists(imagesFolder))
			{
				Directory.CreateDirectory(imagesFolder);
			}

			var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
			var filePath = Path.Combine(imagesFolder, fileName);

			// Lưu hình ảnh vào thư mục
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await image.CopyToAsync(stream);
			}

			// Trả về đường dẫn hình ảnh
			return $"/images/{Path.GetFileName(filePath)}"; // Sửa lại đường dẫn trả về cho đúng
		}
	}
}
