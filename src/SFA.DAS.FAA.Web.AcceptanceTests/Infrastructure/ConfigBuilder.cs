using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;
public static class ConfigBuilder
{
    public static IConfigurationRoot GenerateConfiguration()
    {
        // not tested this yet

        var configSource = new MemoryConfigurationSource
        {
            InitialData = new[]
            {
                new KeyValuePair<string, string>("ConfigurationStorageConnectionString", "UseDevelopmentStorage=true;"),
                new KeyValuePair<string, string>("ConfigNames", "SFA.DAS.FindApprenticeships.Web"),
                new KeyValuePair<string, string>("EnvironmentName", "DEV"),
                new KeyValuePair<string, string>("Version", "1.0"),
                new KeyValuePair<string, string>("FindAnApprenticeship:BaseUrl", "https://localhost:7276"),
                new KeyValuePair<string, string>("FindAnApprenticeshipOuterApi:BaseUrl", "http://localhost:5003"),
                new KeyValuePair<string, string>("FindAnApprenticeshipOuterApi:Key", "")
            }
        };
        var provider = new MemoryConfigurationProvider(configSource);
        return new ConfigurationRoot(new List<IConfigurationProvider>() { provider });
    }
}