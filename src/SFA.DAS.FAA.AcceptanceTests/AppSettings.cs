namespace SFA.DAS.FAA.AcceptanceTests;

public class AppSettings
{
    public string BaseUrl { get; init; } = null!;
    public bool? Headless { get; init; }
    public int? SlowMo { get; init; }
}