using Reqnroll;
using SFA.DAS.FAA.AcceptanceTests.Pages;

namespace SFA.DAS.FAA.AcceptanceTests.Steps;

public class BasicSearch
{
    public string? What { get; set; }
    public string? Where { get; set; }
}

[Binding]
public class SearchStepDefinitions(HomePage homePage, SearchResultsPage searchResultsPage)
{
    [When(@"I visit the HomePage")]
    public async Task WhenIVisitTheHomePage()
    {
        await homePage.VisitAsync();
    }

    [When(@"I search for")]
    public async Task WhenISearchFor(Table table)
    {
        var basicSearch = table.CreateInstance<BasicSearch>();
        await homePage.SearchAsync(basicSearch);
    }

    [Then(@"I am shown the search results")]
    public void ThenIAmShownTheSearchResults()
    {
        searchResultsPage.CheckUrl();
    }

    [Then(@"My search criteria populated in the sidebar")]
    public void ThenMySearchCriteriaPopulatedInTheSidebar()
    {
        searchResultsPage.AssertSearchCriteria();
    }
}