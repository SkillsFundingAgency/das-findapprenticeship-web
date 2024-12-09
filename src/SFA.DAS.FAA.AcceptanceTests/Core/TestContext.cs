using Microsoft.Extensions.Options;
using Reqnroll;
using SFA.DAS.FAA.AcceptanceTests.Models;

namespace SFA.DAS.FAA.AcceptanceTests.Core;

public interface ITestContext
{
    IPage Page { get; }
    AppSettings ApplicationSettings { get; }
    ScenarioContext ScenarioContext { get; }
    
    SearchCriteria? SearchCriteria { get; set; }
}

public class TestContext : ITestContext, IDisposable
{
    public TestContext(Task<IPage> page, IOptions<AppSettings> applicationSettings, ScenarioContext scenarioContext)
    {
        Page = page.Result;
        Page.SetDefaultTimeout(240000);
        ApplicationSettings = applicationSettings.Value;
        ScenarioContext = scenarioContext;
    }

    public void Dispose() => Page.Context.Browser?.CloseAsync();

    public IPage Page { get; }
    public AppSettings ApplicationSettings { get; }
    public ScenarioContext ScenarioContext { get; }
    
    public SearchCriteria? SearchCriteria { get; set; }
}