namespace SFA.DAS.FAA.AcceptanceTests.Pages;

public class SearchResultsPage(ITestContext testContext)
{
    internal const string PageUrl = "apprenticeships";
    private ILocator WhatInput => testContext.Page.GetByLabel("What");
    private ILocator WhereInput => testContext.Page.GetByLabel("Where");
    private ILocator SearchButton => testContext.Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Search" });

    public async Task AssertSearchCriteria()
    {
        testContext.SearchCriteria.Should().NotBeNull();
        (await WhatInput.InputValueAsync()).Should().Be(testContext.SearchCriteria!.What ?? string.Empty);
        (await WhereInput.InputValueAsync()).Should().Be(testContext.SearchCriteria.Where ?? string.Empty);
    }
}