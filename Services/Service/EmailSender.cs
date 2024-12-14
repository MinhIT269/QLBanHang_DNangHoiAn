using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;
using PBL6.Services.IService;
using IEmailSender = PBL6.Services.IService.IEmailSender;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace PBL6.Services.Service
{
    public class EmailSender :  IEmailSender
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUrlHelperFactory _urlHelperFactory; // Add this line

        public EmailSender(
     ICompositeViewEngine viewEngine,
     ITempDataProvider tempDataProvider,
     IServiceProvider serviceProvider,
     IUrlHelperFactory urlHelperFactory)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _urlHelperFactory = urlHelperFactory; // Assign to field
        }

        public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
            var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);
            using var stringWriter = new StringWriter();
            var viewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: false);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"Không tìm thấy view {viewName}");
            }

            var viewDictionary = new ViewDataDictionary<TModel>(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
            {
                Model = model
            };

            var tempData = new TempDataDictionary(httpContext, _tempDataProvider);

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDictionary,
                tempData,
                stringWriter,
                new HtmlHelperOptions())
            {
                // Set the urlHelper to the ViewContext if needed
                ViewData = { { "UrlHelper", urlHelper } }
            };

            await viewResult.View.RenderAsync(viewContext);
            return stringWriter.ToString();
        }


        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "baobap208@gmail.com";
            var pw = "jazd eswg pizs pjzk";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, pw)
            };

            var mailMessage = new MailMessage(from: mail, to: email, subject, message)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mailMessage);
        }

        public async Task SendEmailWithViewAsync<TModel>(string email, string subject, string viewName, TModel model)
        {
            var messageBody = await RenderViewToStringAsync(viewName, model);
            await SendEmailAsync(email, subject, messageBody);
        }
    }
}

