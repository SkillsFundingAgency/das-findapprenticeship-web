using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

namespace SFA.DAS.FAA.Web.AppStart;
[ExcludeFromCodeCoverage]
public static class AddDataProtectionExtensions
{
    private const string SectionName = "FindAnApprenticeship";
    
    public static void AddDataProtection(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection(SectionName)
            .Get<Domain.Configuration.FindAnApprenticeship>();

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