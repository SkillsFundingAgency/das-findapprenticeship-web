using SFA.DAS.FAA.AcceptanceTests.Models;

namespace SFA.DAS.FAA.AcceptanceTests.Core;

public class AppSettings
{
    public string BaseUrl { get; init; } = string.Empty;
    public Environments? Environment  { get; init; }
    public bool? Headless { get; init; }
    public int? SlowMo { get; init; }
    public TestUser? User { get; init; }
}