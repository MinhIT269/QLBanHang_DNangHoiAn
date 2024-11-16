using QLBanHang_API.Dto.Request;

namespace QLBanHang_API.Services.IService
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailRequest emailRequest);
    }
}
