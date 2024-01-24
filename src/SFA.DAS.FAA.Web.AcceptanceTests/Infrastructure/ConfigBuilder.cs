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
                new KeyValuePair<string, string>("ConfigNames", "SFA.DAS.FindApprenticeships.Web,SFA.DAS.Employer.GovSignIn"),
                new KeyValuePair<string, string>("EnvironmentName", "DEV"),
                new KeyValuePair<string, string>("ResourceEnvironmentName", "LOCAL"),
                new KeyValuePair<string, string>("StubAuth", "true"),
                new KeyValuePair<string, string>("Version", "1.0"),
                new KeyValuePair<string, string>("FindAnApprenticeship:BaseUrl", "https://localhost:7276"),
                new KeyValuePair<string, string>("FindAnApprenticeshipOuterApi:BaseUrl", "http://localhost:5027"),
                new KeyValuePair<string, string>("FindAnApprenticeshipOuterApi:Key", ""),
                new KeyValuePair<string, string>("GovUkOidcConfiguration:BaseUrl","https://test-url"),
                new KeyValuePair<string, string>("GovUkOidcConfiguration:ClientId","bd5f0343-7cd7-4ea9-8aa1-63ea8d16ce32"),
                new KeyValuePair<string, string>("GovUkOidcConfiguration:KeyVaultIdentifier","urn:fdc:gov.uk:2022:56P4CMsGh_02YOlWpd8PAOI-2sVlB2nsNU7mcLZYhYw=")
            }
        };
        var provider = new MemoryConfigurationProvider(configSource);
        return new ConfigurationRoot(new List<IConfigurationProvider>() { provider });
    }
}