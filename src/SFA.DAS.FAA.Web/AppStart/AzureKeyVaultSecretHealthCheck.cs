using System.Security.Cryptography.X509Certificates;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Domain.Configuration;

namespace SFA.DAS.FAA.Web.AppStart;

public class AzureKeyVaultSecretHealthCheck(IOptions<FindAnApprenticeshipOuterApi> findAnApprenticeshipOuterApiConfiguration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            
            var credential = new DefaultAzureCredential();
            var secretClient = new SecretClient(new Uri(findAnApprenticeshipOuterApiConfiguration.Value.SecretClientUrl!), credential);
            
            var secret = await secretClient.GetSecretAsync(findAnApprenticeshipOuterApiConfiguration.Value.SecretName!, cancellationToken: cancellationToken);

            if (!secret.HasValue)
            {
                return HealthCheckResult.Degraded($"Has errored - {secret.GetRawResponse().Content.ToDynamicFromJson()}");
            }
                
            var cert = new X509Certificate2(Convert.FromBase64String(secret.Value.Value));
            
            
            return secret.Value != null 
                ? HealthCheckResult.Healthy("Key Vault Secret Client is healthy") 
                : HealthCheckResult.Degraded("Key Vault Secret is missing");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded("Key Vault Secret Client check failed", ex);
        }
    }
}






