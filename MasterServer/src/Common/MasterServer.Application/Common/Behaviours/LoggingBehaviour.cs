using MasterServer.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace MasterServer.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentPlayerService _currentPlayerService;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentPlayerService currentPlayerService)
    {
        _logger = logger;
        _currentPlayerService = currentPlayerService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var playerId = _currentPlayerService.PlayerId;
        var playerName = _currentPlayerService.PlayerName;


        _logger.LogInformation("CleanArchitecture Request: {Name} {@PlayerId} {@PlayerName} {@Request}",
            requestName, playerId, playerName, request);
    }
}
