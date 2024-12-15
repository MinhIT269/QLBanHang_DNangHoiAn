using PBL6.Services.IService;
using QLBanHang_API.Services.IService;
using System.Text.RegularExpressions;

namespace PBL6.Services.Service
{
    public class ImageService : IImageService
    {
        public async Task<string> UploadImageAsync(IFormFile image, Guid productId) //Tai anh len va luu vao thu muc
        {
            // Tạo thư mục dựa trên ProductId
            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "images", productId.ToString());

            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(imagesFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"images/{productId}/{fileName}";
        }

        public async Task<List<string>> GetAllImageUrlsForProductAsync(Guid productId) //lay URL ảnh của sản phẩm dựa trên productId
        {
            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "images", productId.ToString());
            var imageUrls = new List<string>();

            if (Directory.Exists(imagesFolder))
            {
                var files = Directory.GetFiles(imagesFolder);

                foreach (var file in files)
                {
                    var relativeUrl = file
                    .Replace(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar, "")
                    .Replace(Path.DirectorySeparatorChar, '/');
                    var fullUrl = $"{relativeUrl}";
                    imageUrls.Add(fullUrl);
                }
            }

            return await Task.FromResult(imageUrls);
        }

        public async Task<string> UploadImageTempAsync(IFormFile image, HttpContext httpContext) // Tải lên một ảnh tạm thời (temporary) ckeditor
        {
            var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "images", "temp");

            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(tempFolder, fileName);

            using (var temp = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(temp);
            }

            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/temp/{fileName}";
        }

        public async Task<string> UploadImageFromUrlAsync(string imageUrl, Guid productId) // Tải ảnh từ một URL và lưu vào thư mục của sản phẩm
        {
            var httpClient = new HttpClient();
            var imageData = await httpClient.GetByteArrayAsync(imageUrl);

            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "images", productId.ToString());

            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageUrl);
            var filePath = Path.Combine(imagesFolder, fileName);

            await System.IO.File.WriteAllBytesAsync(filePath, imageData);

            return $"images/{productId}/{fileName}";
        }

        public async Task<string> ProcessDescriptionAndUploadImages(string description, Guid productId) // Tìm tất cả các URL ảnh trong một chuỗi HTML mô tả và thay thế bằng các URL đã được tải lên server.
        {
            var matches = Regex.Matches(description, "<img.*?src=\"(.*?)\"", RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    var oldUrl = match.Groups[1].Value;
                    var newUrl = await UploadImageFromUrlAsync(oldUrl, productId);
                    description = description.Replace(oldUrl, newUrl);
                    DeleteTempImage(oldUrl);
                }
            }
            return description;
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), imageUrl.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true; // Xóa thành công
            }

            return false; // File không tồn tại hoặc không thể xóa
        }

        private void DeleteTempImage(string imageUrl) // Xóa ảnh tạm thời trong thư mục temp dựa trên URL
        {
            var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "images", "temp");
            var fileName = Path.GetFileName(new Uri(imageUrl).LocalPath);
            var filePath = Path.Combine(tempFolder, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
