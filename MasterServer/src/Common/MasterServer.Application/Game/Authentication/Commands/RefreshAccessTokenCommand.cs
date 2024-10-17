using MasterServer.Application.Common.Exceptions;
using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;
using MasterServer.Domain.Entities;
using MasterServer.Domain.Mics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace MasterServer.Application.Game.Authentication.Commands
{
    public class RefreshAccessTokenCommand : IRequestWrapper<RefreshTokenResponse>
    {
        public string Version { get; set; }

        public string RefreshToken { get; set; }

        public string RemoteIpAddress { get; set; }

        public string CountryCode { get; set; }
    }

    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandHandler : IRequestHandlerWrapper<RefreshAccessTokenCommand, RefreshTokenResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IDateTimeService _dateTime;
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;
        private readonly IJwtUtils _jwtUtils;
        private readonly IWebsocketNofiticationService _websocketNofiticationService;
        public RefreshTokenCommandHandler(IApplicationDbContext dbContext, IConfiguration configuration, IDateTimeService dateTime, IFirebaseAuthService firebaseAuthService, IIdentityService identityService, IFeatureManagerSnapshot featureHubConfig, ILogger<AuthenticateCommandHandler> logger, IJwtUtils jwtUtils, ICurrentPlayerService currentPlayerService, IWebsocketNofiticationService websocketNofiticationService)
        {
            _dbContext = dbContext;
            _dateTime = dateTime;
            _identityService = identityService;
            _configuration = configuration;
            _logger = logger;
            _jwtUtils = jwtUtils;
            _websocketNofiticationService = websocketNofiticationService;
        }
        public async Task<ServiceResult<RefreshTokenResponse>> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var refreshToken = await _dbContext.RefreshTokens.Include(x => x.Player).Where(x => x.Token == request.RefreshToken).FirstOrDefaultAsync();
                if (refreshToken == null)
                {
                    throw new NotFoundException(nameof(RefreshToken), $"refreshtoken_{request.RefreshToken}");
                }

                if (!refreshToken.IsActive)
                {
                    throw new ValidationException("refreshtoken", "refreshtoken_is_not_active");
                }

                var player = refreshToken.Player;
                refreshToken = _jwtUtils.GenerateRefreshToken(request.RemoteIpAddress.ToString(), true);
                player.RefreshTokens.Add(refreshToken);
                await _dbContext.RefreshTokens.Where(x => x.Player == player).OrderByDescending(x => x.Expires).Skip(5).ExecuteDeleteAsync();

                _dbContext.PlayerActionLogs.Add(new Domain.Entities.ActionLogs.PlayerActionLog(player.Id, GameAction.Update, GameSubAction.RefreshToken, GameSubject.User, new Dictionary<string, object>()
                {
                    { GameActionLogParamaterKey.GameVersion,request.Version },
                    {GameActionLogParamaterKey.IpAddress, request.RemoteIpAddress.ToString()}
                }));

                await _dbContext.SaveChangesAsync();
                var jwtToken = _jwtUtils.GenerateJwtToken(request.Version, player);

                var playerLoginResponse = new PlayerLoginResponse()
                {
                    AccessToken = jwtToken,
                    RefreshToken = refreshToken.Token,
                };
                return ServiceResult.Success(playerLoginResponse);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failed<PlayerLoginResponse>(ServiceError.AuthenticationCredentialIsNotCorrect);
            }
        }
    }

}
