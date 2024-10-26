namespace QLBanHang_API.Services.IService
{
	public interface IImageService
	{
		Task<string> UploadImageAsync(IFormFile image);
	}
}
