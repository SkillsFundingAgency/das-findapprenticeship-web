using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Reqnroll;
using SFA.DAS.FAA.AcceptanceTests.Core;

namespace SFA.DAS.FAA.AcceptanceTests;

public interface ITestContext
{
    IPage Page { get; }
    IOptions<AppSettings> ApplicationSettings { get; }
    ScenarioContext ScenarioContext { get; }
    
    SearchCriteria? SearchCriteria { get; set; }
}

public class TestContext : ITestContext, IDisposable
{
    public TestContext(Task<IPage> page, IOptions<AppSettings> applicationSettings, ScenarioContext scenarioContext)
    {
        Page = page.Result;
        Page.SetDefaultTimeout(240000);
        ApplicationSettings = applicationSettings;
        ScenarioContext = scenarioContext;
    }

    public void Dispose()
    {
        Page.Context.Browser?.CloseAsync();
    }

    public IPage Page { get; }
    public IOptions<AppSettings> ApplicationSettings { get; }
    public ScenarioContext ScenarioContext { get; }
    
    public SearchCriteria? SearchCriteria { get; set; }
}