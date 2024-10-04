using MasterServer.Application.Common.Models;
using MasterServer.Domain.Entities;
using System.Security.Claims;

namespace MasterServer.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> IsInRoleAsync(ClaimsPrincipal? principal, string role);
    Task<bool> AuthorizeAsync(ClaimsPrincipal? principal, string policyName);
    Task<ServiceResult<Player>> GetOrCreatePlayerByFirebasePlayerId(string username, string password);
    Task<ServiceResult<Player>> GetOrCreatePlayerByGoogle(string token);

    Task<ServiceResult<Player>> GetOrCreatePlayerByFacebook(string token);
}


