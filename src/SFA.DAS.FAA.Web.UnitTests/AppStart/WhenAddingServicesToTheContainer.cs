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
using SFA.DAS.FAT.Domain.Interfaces;
namespace SFA.DAS.FAA.Web.UnitTests.AppStart;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(IApiClient))]
    [TestCase(typeof(IDateTimeService))]
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
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}