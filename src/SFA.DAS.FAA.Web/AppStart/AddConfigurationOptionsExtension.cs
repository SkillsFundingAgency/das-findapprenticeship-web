using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Domain.Configuration;

namespace SFA.DAS.FAA.Web.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<Domain.Configuration.FindAnApprenticeship>(configuration.GetSection(nameof(FindAnApprenticeship)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<Domain.Configuration.FindAnApprenticeship>>().Value);
        services.Configure<FindAnApprenticeshipOuterApi>(configuration.GetSection(nameof(FindAnApprenticeshipOuterApi)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<FindAnApprenticeshipOuterApi>>().Value);
    }
}