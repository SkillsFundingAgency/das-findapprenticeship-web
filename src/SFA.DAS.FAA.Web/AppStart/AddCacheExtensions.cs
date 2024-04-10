using System;

namespace SFA.DAS.FAA.Web.AppStart
{
    public static class AddCacheExtensions
    {
        public static void AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection(nameof(FindAnApprenticeship))
                .Get<Domain.Configuration.FindAnApprenticeship>();

            if (config == null) return;

            if (configuration["ResourceEnvironmentName"].ToLower() == "local")
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache(
                    options => { options.Configuration = config.RedisConnectionString; });
            }
        }
    }
}
