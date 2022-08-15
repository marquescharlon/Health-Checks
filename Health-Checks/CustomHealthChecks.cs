using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace Health_Checks
{
    public class CustomHealthChecks : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new HealthCheckResult(
                status: HealthStatus.Unhealthy,
                description: "API está doente"
            ));
        }
    }
}
