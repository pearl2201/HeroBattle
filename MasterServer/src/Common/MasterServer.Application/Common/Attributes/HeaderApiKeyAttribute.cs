using MasterServer.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MasterServer.Application.Common.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class HeaderApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _keyName;
        public HeaderApiKeyAttribute(string keyName)
        {
            _keyName = keyName;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var extractedApiKey))
            {

                var details = ServiceResult.Failed(ServiceError.ForbiddenError);

                context.Result = new UnauthorizedObjectResult(details);
                return;
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var apiKey = appSettings[_keyName];

            if (!apiKey.Equals(extractedApiKey))
            {
                var details = ServiceResult.Failed(ServiceError.ForbiddenError);

                context.Result = new UnauthorizedObjectResult(details);
                return;
            }

            await next();
        }
    }

}
