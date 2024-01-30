using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Application.Queries.BrowseByInterestsLocation;
using SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAT.Domain.Interfaces;
namespace SFA.DAS.FAA.Web.UnitTests.AppStart;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(IApiClient))]
    [TestCase(typeof(IDateTimeService))]
    [TestCase(typeof(IDataProtectorService))]
    [TestCase(typeof(IRequestHandler<GetBrowseByInterestsQuery, GetBrowseByInterestsResult>))]
    [TestCase(typeof(IRequestHandler<GetBrowseByInterestsLocationQuery, GetBrowseByInterestsLocationQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetLocationsBySearchQuery, GetLocationsBySearchQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetSearchApprenticeshipsIndexQuery, GetSearchApprenticeshipsIndexResult>))]
    public void Then_The_Dependencies_Are_Correctly_Resolved(Type toResolve)
    {
        var serviceCollection = new ServiceCollection();
        SetupServiceCollection(serviceCollection);
        var provider = serviceCollection.BuildServiceProvider();

        var type = provider.GetService(toResolve);
        Assert.That(type, Is.Not.Null);
    }

    private static void SetupServiceCollection(ServiceCollection serviceCollection)
    {
        var configuration = GenerateConfiguration();
            
        serviceCollection.AddSingleton(Mock.Of<IWebHostEnvironment>());
        serviceCollection.AddSingleton(Mock.Of<IConfiguration>());
        serviceCollection.AddConfigurationOptions(configuration);
        serviceCollection.AddDistributedMemoryCache();
        serviceCollection.AddServiceRegistration();
        serviceCollection.AddAuthenticationServices(configuration);
    }

    private static IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("FindAnApprenticeshipOuterApi:BaseUrl", "https://test.com/"),
                new KeyValuePair<string, string>("FindAnApprenticeshipOuterApi:Key", "123edc"),
                new KeyValuePair<string, string>("EnvironmentName", "test"),
                new KeyValuePair<string, string>("ResourceEnvironmentName", "local"),
                new KeyValuePair<string, string>("StubAuth", "true"),
                new KeyValuePair<string, string>("GovUkOidcConfiguration:BaseUrl","https://test-url"),
                new KeyValuePair<string, string>("GovUkOidcConfiguration:ClientId","bd5f0343-7cd7-4ea9-8aa1-63ea8d16ce32"),
                new KeyValuePair<string, string>("GovUkOidcConfiguration:KeyVaultIdentifier","urn:fdc:gov.uk:2022:56P4CMsGh_02YOlWpd8PAOI-2sVlB2nsNU7mcLZYhYw=")
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}