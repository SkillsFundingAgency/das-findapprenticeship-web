namespace SFA.DAS.FAA.Domain.Configuration;

public class FindAnApprenticeship
{
    public string? DataProtectionKeysDatabase { get; set; }
    public string? RedisConnectionString { get; set; }
    public string? GoogleMapsApiKey { get; set; }
    public string? GoogleMapsId { get; set; }
    public bool AccountDeletionFeature { get; set; } = false;
}