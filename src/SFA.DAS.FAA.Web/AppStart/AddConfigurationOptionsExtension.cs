using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Domain.Configuration;

namespace SFA.DAS.FAA.Web.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<FindAnApprenticeshipWebConfiguration>(configuration.GetSection(nameof(FindAnApprenticeshipWebConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<FindAnApprenticeshipWebConfiguration>>().Value);
        services.Configure<FindAnApprenticeshipApi>(configuration.GetSection(nameof(FindAnApprenticeshipApi)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<FindAnApprenticeshipApi>>().Value);
    }
}