using QLBanHang_API.Dto.Request;

namespace PBL6.Services.IService
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailRequest emailRequest);
    }
}
