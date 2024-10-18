using MasterServer.Application.Common.Models;
using MasterServer.Domain.Entities;
using System.Security.Claims;

namespace MasterServer.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> IsInRoleAsync(ClaimsPrincipal? principal, string role);
    Task<bool> AuthorizeAsync(ClaimsPrincipal? principal, string policyName);
    Task<ServiceResult<Player>> GetOrCreatePlayerByDeviceId(string deviceId);
    Task<ServiceResult<Player>> GetOrCreatePlayerByCredentials(string username, string password);
    Task<ServiceResult<Player>> GetOrCreatePlayerByGoogle(string token);
    Task<ServiceResult<Player>> GetOrCreatePlayerByFacebook(string token);

    Task<IdentityLoginResult> ContinueLoginFlow(Player player, string version, string countryCode, string ipAddress);
}


public struct IdentityLoginResult
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public string SessionId { get; set; }
}

