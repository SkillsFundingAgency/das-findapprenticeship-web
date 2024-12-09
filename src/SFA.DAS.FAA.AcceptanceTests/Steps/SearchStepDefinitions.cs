using Reqnroll;
using SFA.DAS.FAA.AcceptanceTests.Pages;

namespace SFA.DAS.FAA.AcceptanceTests.Steps;

public class BasicSearch
{
    public string? What { get; set; }
    public string? Where { get; set; }
}

[Binding]
public class SearchStepDefinitions(ITestContext testContext, HomePage homePage, SearchResultsPage searchResultsPage)
{
    [Given(@"I visit the HomePage")]
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
        var uri = new Uri(testContext.Page.Url);
        uri.AbsolutePath.Should().EndWith($"/{SearchResultsPage.PageUrl}");
    }

    [Then(@"my search criteria are populated in the sidebar")]
    public async Task ThenMySearchCriteriaArePopulatedInTheSidebar()
    {
        testContext.SearchCriteria.Should().NotBeNull();
        (await searchResultsPage.WhatInput.InputValueAsync()).Should().Be(testContext.SearchCriteria!.What ?? string.Empty);
        (await searchResultsPage.WhereInput.InputValueAsync()).Should().Be(testContext.SearchCriteria.Where ?? string.Empty);
    }
}