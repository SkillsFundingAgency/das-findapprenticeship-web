namespace SFA.DAS.FAA.Domain.Configuration;

public class FindAnApprenticeshipOuterApi
{
    public string? Key { get; set; }
    public string? BaseUrl { get; set; }
    public string? BaseUrlSecure { get; set; }
    public string? SecretClientUrl { get; set; }
    public string? SecretName { get; set; }
    public bool UseSecureGateway { get; set; }
}