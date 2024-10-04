using MasterServer.Application.Common.Attributes;
using MasterServer.Application.Common.Exceptions;
using MediatR;
using Microsoft.FeatureManagement;
using System.Reflection;

namespace MasterServer.Application.Common.Behaviours
{
    public class FeatureHubGateBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {

        private readonly IFeatureManagerSnapshot _featureHubConfig;

        public FeatureHubGateBehaviour(IFeatureManagerSnapshot featureHubConfig)
        {

            _featureHubConfig = featureHubConfig;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<FeatureHubGateAttribute>();

            if (authorizeAttributes.Any())
            {

                foreach (var attr in authorizeAttributes)
                {
                    var _features = attr.Features;
                    if (_features != null && _features.Count() > 0)
                    {
                        foreach (var feature in _features)
                        {
                            if (!await _featureHubConfig.IsEnabledAsync(feature).ConfigureAwait(false))
                            {

                                throw new UnavailableFeatureException(feature);
                            }
                        }
                    }
                }

            }

            return await next();
        }
    }

}
