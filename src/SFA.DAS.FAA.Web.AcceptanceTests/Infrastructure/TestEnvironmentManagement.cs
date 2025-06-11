using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.MockServer.MockServerBuilder;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using TechTalk.SpecFlow;
using WireMock.Server;
using Constants = SFA.DAS.FAA.Web.AcceptanceTests.Data.Constants;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;

[Binding]
public sealed class TestEnvironmentManagement
{
    private readonly ScenarioContext _context;
    private static ITestHttpClient _testHttpClient;
    private static IWireMockServer _staticApiServer;
    private Mock<IApiClient> _mockApiClient;
    private readonly string? _environment;
    private readonly string? _outerApi;
    private static TestServer _server;

    public TestEnvironmentManagement(ScenarioContext context)
    {
        _context = context;
        var environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT");
        _outerApi = Environment.GetEnvironmentVariable("OUTERAPI");

        if (!string.IsNullOrEmpty(environmentName))
        {
            _environment = $"https://{DomainExtensions.GetDomain(environmentName)}";
            _context.Set(environmentName, ContextKeys.EnvironmentName);
        }
        else
        {
            _environment = null;
            _context.Set("Local", ContextKeys.EnvironmentName);
        }
        Console.WriteLine($"Environment: {_environment}");
    }

    [BeforeScenario("WireMockServer", Order = 0)]
    public void StartWebApp()
    {
        if (!string.IsNullOrEmpty(_environment))
        {
            return;
        }
        _staticApiServer = MockApiServer.Start();
        var webApp = new CustomWebApplicationFactory<SearchApprenticeshipsController>()
            .WithWebHostBuilder(c=>c.UseEnvironment("IntegrationTest").UseConfiguration(ConfigBuilder.GenerateConfiguration()));
        _server = webApp.Server;

        _testHttpClient = new TestHttpClient(_server);

        _context.Set(_server, ContextKeys.TestServer);
        _context.Set(_testHttpClient, ContextKeys.TestHttpClient);
    }
    
    [BeforeScenario("RunOnEnvironment", Order = 1)]
    public void SetupRunOnEnvironment()
    {
        if (string.IsNullOrEmpty(_environment))
        {
            return;
        }
        _context.Get<string>(ContextKeys.EnvironmentName);
        _context.Set(_environment, ContextKeys.CurrentEnvironment);

        _testHttpClient = new AcceptanceTestHttpClient(_environment);

        _context.Set<TestServer>(null!, ContextKeys.TestServer);
        _context.Set(_testHttpClient, ContextKeys.TestHttpClient);
    }

    [BeforeScenario("RunOnEnvironment:([A-Za-z]+)", Order = 1)]
    public void SetupRunOnSpecificEnvironment(string requiredEnvironment)
    {
        if (string.IsNullOrEmpty(_environment))
        {
            return;
        }
        var environmentName = _context.Get<string>(ContextKeys.EnvironmentName);
        Console.WriteLine($"Scenario requires environment: {requiredEnvironment}. Current configured environment: {environmentName ?? "N/A"}");

        // Normalize the required environment for comparison (e.g., "AT" vs "at")
        string normalizedRequiredEnv = requiredEnvironment.Trim().ToUpperInvariant();
        string normalizedCurrentEnv = environmentName?.Trim().ToUpperInvariant() ?? "LOCAL";

        if (!normalizedCurrentEnv.Equals(normalizedRequiredEnv, StringComparison.OrdinalIgnoreCase))
        {
            var skipReason = $"Skipping scenario: Requires environment '{requiredEnvironment}', but currently running on '{environmentName ?? "Local"}'.";
            Console.WriteLine(skipReason);

            throw new PendingStepException(skipReason);
        }

        _context.Set(_environment, ContextKeys.CurrentEnvironment);

        _testHttpClient = new AcceptanceTestHttpClient(_environment);

        _context.Set<TestServer>(null!, ContextKeys.TestServer);
        _context.Set(_testHttpClient, ContextKeys.TestHttpClient);
    }

    [BeforeScenario("ApiContract")]
    public void SetupContractTests()
    {
        var apiSpecUrl = "";
        if (!string.IsNullOrEmpty(_outerApi))
        {
            apiSpecUrl = $"https://{_outerApi}/swagger/v1/swagger.json";
        }
        _context.Set(apiSpecUrl, ContextKeys.ApiSpecUrl);
    }

    [BeforeScenario("MockApiClient")]
    public void StartWebAppWithMockApiClient()
    {
        _mockApiClient = new Mock<IApiClient>();

        _mockApiClient.Setup(x => x.Get<SearchApprenticeshipsApiResponse>(It.IsAny<GetSearchApprenticeshipsIndexApiRequest>()))
            .ReturnsAsync(new SearchApprenticeshipsApiResponse());

        _mockApiClient.Setup(x => x.Get<BrowseByInterestsApiResponse>(It.IsAny<GetBrowseByInterestsApiRequest>()))
            .ReturnsAsync(new BrowseByInterestsApiResponse());

        _server = new TestServer(new WebHostBuilder()
            .ConfigureTestServices(services => ConfigureTestServices(services, _mockApiClient))
            .UseEnvironment(Environments.Development)
            .UseStartup<SearchApprenticeshipsController>()
            .UseConfiguration(ConfigBuilder.GenerateConfiguration()));

        _context.Set(_mockApiClient, ContextKeys.MockApiClient);
    }

    [BeforeScenario("NewApplication")]
    public async Task NewApplication()
    {
        _context.Set(Constants.NewVacancyReference, ContextKeys.VacancyReference);
        _context.Set(Constants.NewApplicationId, ContextKeys.ApplicationId);
    }

    [BeforeScenario("ExistingApplication")]
    public async Task ExistingApplication()
    {
        _context.Set(Constants.ExistingVacancyReference, ContextKeys.VacancyReference);
        _context.Set(Constants.ExistingApplicationId, ContextKeys.ApplicationId);
    }

    [BeforeScenario("AuthenticatedUser", Order = 2)]
    public async Task AuthenticatedUser()
    {
        var client = _context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);

        Dictionary<string, string> formData;

        bool isRunOnEnvironmentPresent = _context.ScenarioInfo.Tags
            .Any(t => t.Equals("RunOnEnvironment", StringComparison.OrdinalIgnoreCase));

        string environmentName = "Local";
        if(_context.TryGetValue(ContextKeys.EnvironmentName, out string storedEnvironmentName))
        {
            environmentName = storedEnvironmentName;
        }

        if (isRunOnEnvironmentPresent && environmentName.Equals("AT", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("AuthenticatedUser: Using AT environment-specific credentials.");
            formData = new Dictionary<string, string>
            {
                { "Id", MockServer.Constants.CandidateOnAT },
                { "Email", "gfshjeadgsfdbshjkcx@mailinator.com" },
                { "MobilePhone", "12345 67890" }
            };
        }
        else
        {
            Console.WriteLine($"AuthenticatedUser: Using default credentials (Environment: {environmentName}).");
            // Default user data for local/development or when RunOnEnvironment is not applied
            formData = new Dictionary<string, string>
            {
                { "Id", MockServer.Constants.CandidateIdWithApplications },
                { "Email", "test@test.com" },
                { "MobilePhone", "12345 67890" }
            };
        }

        await client.PostAsync("/account-details", formData);
    }
    
    [BeforeScenario("AuthenticatedUserWithIncompleteSetup")]
    public async Task AuthenticatedUserWithIncompleteSetup()
    {
        var client = _context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);

        var formData = new Dictionary<string, string>
        {
            { "Id", MockServer.Constants.CandidateIdIncomplete },
            { "Email", "test@test.com" },
            { "MobilePhone", "12345 67890" }
        };

        await client.PostAsync("/account-details", formData);
    }

    [AfterScenario("WireMockServer")]
    public void StopEnvironment()
    {
        if (!string.IsNullOrEmpty(_environment))
        {
            return;
        }
        _server
            .Dispose();
        _staticApiServer?.Stop();
        _staticApiServer?.Dispose();
        _testHttpClient?.Dispose();
    }

    [AfterScenario("MockApiClient")]
    public void StopTestEnvironment()
    {
        _server.Dispose();
    }

    private void ConfigureTestServices(IServiceCollection serviceCollection, Mock<IApiClient> mockApiClient)
    {
        foreach (var descriptor in serviceCollection.Where(
            d => d.ServiceType ==
                 typeof(IApiClient)).ToList())
        {
            serviceCollection.Remove(descriptor);
        }
        serviceCollection.AddSingleton(mockApiClient);
        serviceCollection.AddSingleton(mockApiClient.Object);
    }
}