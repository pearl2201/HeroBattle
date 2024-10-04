using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Game;
using MediatR;

namespace MasterServer.Application.Common.Behaviours;

public class InjectPlayerIdToRequestBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ICurrentPlayerService _currentPlayerService;

    public InjectPlayerIdToRequestBehaviour(
        ICurrentPlayerService currentPlayerService)
    {
        _currentPlayerService = currentPlayerService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is GameRequestBase cmdBase && _currentPlayerService.PlayerSubId.HasValue && !cmdBase.PlayerId.HasValue)
        {
            cmdBase.PlayerId = _currentPlayerService.PlayerSubId;
        }

        // Player is authorized / authorization not required
        return await next();
    }
}
