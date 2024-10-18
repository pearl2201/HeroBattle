using FluentValidation;
using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;
using Microsoft.Extensions.Logging;
using ValidationException = MasterServer.Application.Common.Exceptions.ValidationException;

namespace MasterServer.Application.Game.Authentication.Commands
{
    public class LoginCredentialsCommand : BaseLoginCommand
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class LoginCredentialsCommandValidator : AbstractValidator<LoginCredentialsCommand>
    {
        public LoginCredentialsCommandValidator()
        {
            RuleFor(v => v.UserName).NotEmpty().MinimumLength(6);
            RuleFor(v => v.Password).NotEmpty().MinimumLength(8).MaximumLength(32);
        }
    }
    public class LoginCredentialsCommandHandler : IRequestHandlerWrapper<LoginCredentialsCommand, PlayerLoginResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly ILogger<LoginCredentialsCommandHandler> _logger;

        public LoginCredentialsCommandHandler(IApplicationDbContext dbContext, IIdentityService identityService, ILogger<LoginCredentialsCommandHandler> logger)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _logger = logger;
        }
        public async Task<ServiceResult<PlayerLoginResponse>> Handle(LoginCredentialsCommand request, CancellationToken cancellationToken)
        {
            var getUserResult = await _identityService.GetOrCreatePlayerByCredentials(request.UserName, request.Password);
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
