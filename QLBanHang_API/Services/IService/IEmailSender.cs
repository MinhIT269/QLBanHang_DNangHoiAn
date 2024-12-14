namespace PBL6.Services.IService
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string email,string subject,string message);
        public  Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
        public Task SendEmailWithViewAsync<TModel>(string email, string subject, string viewName, TModel model);
    }
}
