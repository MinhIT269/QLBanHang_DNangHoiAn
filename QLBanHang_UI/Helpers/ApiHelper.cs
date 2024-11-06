using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using QLBanHang_UI.Models.Request;
namespace QLBanHang_UI.Helpers
{
    public class ApiHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Phương thức lấy dữ liệu từ API
        public async Task<T?> GetDataFromApi<T>(string url, string errorMessage) where T : class
        {
            try
            {
                var client = _httpClientFactory.CreateClient(); // Tạo client HTTP
                var httpResponse = await client.GetAsync(url);  // Gửi yêu cầu GET đến URL

                if (httpResponse.IsSuccessStatusCode)
                {
                    return await httpResponse.Content.ReadFromJsonAsync<T>(); // Đọc và trả về dữ liệu từ nội dung phản hồi
                }
                else
                {
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception)
            {
                throw new Exception("Đã xảy ra lỗi khi tải dữ liệu.");
            }
        }
    }
}
