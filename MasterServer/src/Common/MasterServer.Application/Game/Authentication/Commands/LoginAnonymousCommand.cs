
using FluentValidation;
using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;
using Microsoft.Extensions.Logging;
using ValidationException = MasterServer.Application.Common.Exceptions.ValidationException;

namespace MasterServer.Application.Game.Authentication.Commands
{
    public class BaseLoginCommand : IRequestWrapper<PlayerLoginResponse>
    {
        public string Version { get; set; }

        public string DeviceId { get; set; }

        public string RemoteIpAddress { get; set; }

        public string CountryCode { get; set; }
    }
    public class LoginAnonymousCommand : BaseLoginCommand
    {

    }

    public class LoginAnonymousValidator : AbstractValidator<LoginAnonymousCommand>
    {
        public LoginAnonymousValidator()
        {
            RuleFor(v => v.DeviceId).NotEmpty();
        }
    }


    public record PlayerLoginResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public long Id { get; set; }

        public string UserName { get; set; }

        public string SessionId { get; set; }
    }
    public class LoginAnonymousCommandHandler : IRequestHandlerWrapper<LoginAnonymousCommand, PlayerLoginResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly ILogger<LoginAnonymousCommandHandler> _logger;

        public LoginAnonymousCommandHandler(IApplicationDbContext dbContext, IIdentityService identityService, ILogger<LoginAnonymousCommandHandler> logger)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _logger = logger;
        }
        public async Task<ServiceResult<PlayerLoginResponse>> Handle(LoginAnonymousCommand request, CancellationToken cancellationToken)
        {
            var getUserResult = await _identityService.GetOrCreatePlayerByDeviceId(request.DeviceId);
            if (!getUserResult.Succeeded)
            {
                throw new ValidationException("player", $"{request.DeviceId}");
            }
            var player = getUserResult.Data;
            if (player.LockKind == Domain.Enums.PlayerLockKind.Permanent)
            {
                return ServiceResult.Failed<PlayerLoginResponse>(ServiceError.PlayerIsBan);
            }
            var identityResult = await _identityService.ContinueLoginFlow(player, request.Version, request.CountryCode, request.RemoteIpAddress);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            PlayerLoginResponse playerLoginResponse = new PlayerLoginResponse()
            {
                AccessToken = identityResult.AccessToken,
                RefreshToken = identityResult.RefreshToken,
                Id = player.Id,
                UserName = player.NickName,
                SessionId = identityResult.SessionId
            };

            return ServiceResult.Success(playerLoginResponse);
        }
    }
}
