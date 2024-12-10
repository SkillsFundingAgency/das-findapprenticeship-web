using Reqnroll;
using SFA.DAS.FAA.AcceptanceTests.Core;
using SFA.DAS.FAA.AcceptanceTests.Models;
using SFA.DAS.FAA.AcceptanceTests.Pages;

namespace SFA.DAS.FAA.AcceptanceTests.Steps;

[Binding]
public class SearchResultsPageSteps(ITestContext testContext, SearchResultsPage searchResultsPage)
{
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
    
    [Given(@"I search for")]
    [When(@"I search for")]
    public async Task WhenISearchFor(Table table)
    {
        var searchCriteria = table.CreateInstance<SearchCriteria>();
        
        await searchResultsPage.VisitAsync();
        await searchResultsPage.SearchAsync(searchCriteria);
    }

    [When(@"save the first vacancy in the search results")]
    public void WhenSaveTheFirstVacancyInTheSearchResults()
    {
        ScenarioContext.StepIsPending();
    }
}