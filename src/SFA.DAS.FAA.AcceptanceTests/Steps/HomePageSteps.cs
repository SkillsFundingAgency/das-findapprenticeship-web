using Reqnroll;
using SFA.DAS.FAA.AcceptanceTests.Core;
using SFA.DAS.FAA.AcceptanceTests.Models;
using SFA.DAS.FAA.AcceptanceTests.Pages;

namespace SFA.DAS.FAA.AcceptanceTests.Steps;

[Binding]
public class HomePageSteps(ITestContext testContext, HomePage homePage)
{
    [Given(@"I visit the HomePage")]
    [When(@"I visit the HomePage")]
    public async Task WhenIVisitTheHomePage()
    {
        await homePage.VisitAsync();
    }
    
    [When(@"I search from the homepage for")]
    public async Task WhenISearchFor(Table table)
    {
        var searchCriteria = table.CreateInstance<SearchCriteria>();
        
        await homePage.VisitAsync();
        await homePage.SearchAsync(searchCriteria);
    }
}