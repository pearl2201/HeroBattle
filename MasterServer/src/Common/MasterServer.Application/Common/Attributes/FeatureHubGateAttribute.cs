using MasterServer.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace MasterServer.Application.Common.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class FeatureHubGateAttribute : ActionFilterAttribute, IAsyncActionFilter
    {


        private readonly IEnumerable<string> _features;

        public FeatureHubGateAttribute(string feature)
        {
            _features = new string[] { feature };
        }

        public FeatureHubGateAttribute(string[] features)
        {
            _features = features;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (_features != null && _features.Count() > 0)
            {
                var featureHubConfig = context.HttpContext.RequestServices.GetRequiredService<IFeatureManagerSnapshot>();

                foreach (var feature in _features)
                {
                    if (!(await featureHubConfig.IsEnabledAsync(feature).ConfigureAwait(false)))
                    {
                        throw new UnavailableFeatureException(feature);
                    }
                }

            }


            await next();
        }

        public IEnumerable<string> Features => _features;
    }

}
