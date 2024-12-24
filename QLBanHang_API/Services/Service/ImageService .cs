using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QLBanHang_API.Services.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    // Inject IConfiguration để lấy thông tin cấu hình từ appsettings.json
    public ImageService(IConfiguration configuration)
    {
        var cloudinaryConfig = configuration.GetSection("Cloudinary");

        var cloudinaryAccount = new Account(
            cloudinaryConfig["CloudName"],
            cloudinaryConfig["ApiKey"],
            cloudinaryConfig["ApiSecret"]
        );

        _cloudinary = new Cloudinary(cloudinaryAccount);
    }

    // Tải lên hình ảnh lên Cloudinary và trả về URL
    public async Task<string> UploadImageAsync(IFormFile image, Guid productId)
    {
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(fileName, image.OpenReadStream()), // Dùng stream của file tải lên
            PublicId = $"products/{productId}/{fileName}" // Đặt PublicId cho ảnh
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult?.SecureUrl?.ToString(); // Trả về URL ảnh sau khi upload
    }

    // Lấy tất cả các URL của ảnh sản phẩm từ Cloudinary
    public async Task<List<string>> GetAllImageUrlsForProductAsync(Guid productId)
    {
        var imageUrls = new List<string>();

        // Sử dụng ListResourcesParams để lọc ảnh theo một số thông số khác
        var listParams = new ListResourcesParams()
        {
            MaxResults = 100, // Bạn có thể điều chỉnh số lượng kết quả tối đa
            ResourceType = ResourceType.Image // Chỉ tìm ảnh
        };

        // Lấy danh sách tài nguyên từ Cloudinary
        var resources = await _cloudinary.ListResourcesAsync(listParams);

        foreach (var resource in resources.Resources)
        {
            // Kiểm tra xem ảnh có chứa phần "products/{productId}" trong PublicId
            if (resource.PublicId.Contains($"products/{productId}/"))
            {
                imageUrls.Add(resource.SecureUrl.ToString());
            }
        }

        return imageUrls;
    }

    // Tải lên hình ảnh tạm thời (temporary) cho CKEditor
    public async Task<string> UploadImageTempAsync(IFormFile image, HttpContext httpContext)
    {
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(fileName, image.OpenReadStream()),
            PublicId = $"temp/{fileName}" // Lưu vào thư mục tạm thời
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult?.SecureUrl?.ToString(); // Trả về URL ảnh tạm thời
    }

    // Tải ảnh từ URL và lưu vào Cloudinary
    public async Task<string> UploadImageFromUrlAsync(string imageUrl, Guid productId)
    {
        var httpClient = new HttpClient();
        var imageData = await httpClient.GetByteArrayAsync(imageUrl);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageUrl);

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(fileName, new MemoryStream(imageData)),
            PublicId = $"products/{productId}/{fileName}" // Đặt PublicId cho ảnh
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult?.SecureUrl?.ToString(); // Trả về URL ảnh sau khi tải lên
    }

    // Xử lý mô tả và thay thế các URL ảnh trong mô tả với URL đã tải lên Cloudinary
    public async Task<string> ProcessDescriptionAndUploadImages(string description, Guid productId)
    {
        var matches = Regex.Matches(description, "<img.*?src=\"(.*?)\"", RegexOptions.IgnoreCase);

        foreach (Match match in matches)
        {
            if (match.Groups.Count > 1)
            {
                var oldUrl = match.Groups[1].Value;
                var newUrl = await UploadImageFromUrlAsync(oldUrl, productId);
                description = description.Replace(oldUrl, newUrl);
            }
        }

        return description;
    }

    // Xóa ảnh khỏi Cloudinary
    public async Task<bool> DeleteImageAsync(string imageUrl)
    {
        var deleteParams = new DeletionParams(imageUrl); // Sử dụng DeletionParams để xóa ảnh

        var deletionResult = await _cloudinary.DestroyAsync(deleteParams);

        return deletionResult?.Result == "ok"; // Trả về true nếu xóa thành công
    }
}
