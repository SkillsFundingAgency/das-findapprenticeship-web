using Microsoft.AspNetCore.DataProtection;
using SFA.DAS.FAA.Domain.Configuration;
using StackExchange.Redis;

namespace SFA.DAS.FAA.Web.AppStart;

public static class AddDataProtectionExtensions
{
    public static void AddDataProtection(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(FindAnApprenticeshipWebConfiguration))
            .Get<FindAnApprenticeshipWebConfiguration>();

        if (config != null 
            && !string.IsNullOrEmpty(config.DataProtectionKeysDatabase) 
            && !string.IsNullOrEmpty(config.RedisConnectionString))
        {
            var redisConnectionString = config.RedisConnectionString;
            var dataProtectionKeysDatabase = config.DataProtectionKeysDatabase;

            var configurationOptions = ConfigurationOptions.Parse($"{redisConnectionString},{dataProtectionKeysDatabase}");
            var redis = ConnectionMultiplexer
                .Connect(configurationOptions);

            services.AddDataProtection()
                .SetApplicationName("das-apprentice")
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
        }
    }
}