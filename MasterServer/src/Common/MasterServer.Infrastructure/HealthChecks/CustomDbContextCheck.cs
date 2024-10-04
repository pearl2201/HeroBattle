using MasterServer.Application.Common.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MasterServer.Infrastructure.HealthChecks
{
    public class CustomDbContextCheck : IHealthCheck
    {
        private readonly IApplicationDbContext _dbContext;

        public CustomDbContextCheck(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {

                //var _ = await _dbContext.Players.AnyAsync(cancellationToken);
                return HealthCheckResult.Healthy("db check success");
            }
            catch (Exception exception)
            {
                return HealthCheckResult.Unhealthy(exception.Message, exception);
            }
        }
    }
}
