using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Vacancies;

namespace SFA.DAS.FAA.Web.AppStart;

public class SearchIndexStatisticsHealthCheck(IApiClient apiClient) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await apiClient.Get<GetSearchIndexStatisticsResponse?>(new GetSearchIndexStatisticsRequest());
            if (response is null)
            {
                return HealthCheckResult.Degraded("Azure search index is unhealthy");
            }

            var data = new Dictionary<string, object>
            {
                { "creationDate", response.CreatedDate },
                { "lastUpdatedDate", response.LastUpdatedDate },
            };

            foreach (var stat in response.IndexStatistics)
            {
                data.Add($"{stat.Source.ToLower()}Adverts", stat.Count);
            }
            
            return HealthCheckResult.Healthy("Azure search index is healthy", data);
        }
        catch (Exception e)
        {
            return HealthCheckResult.Degraded("Azure search index is unhealthy", e);
        }
    }
}