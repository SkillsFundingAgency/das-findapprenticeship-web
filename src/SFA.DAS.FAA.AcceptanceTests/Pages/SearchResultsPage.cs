namespace SFA.DAS.FAA.AcceptanceTests.Pages;

public class SearchResultsPage(ITestContext testContext)
{
    internal const string PageUrl = "apprenticeships";
    public ILocator WhatInput => testContext.Page.GetByLabel("What");
    public ILocator WhereInput => testContext.Page.GetByLabel("Where");
    //private ILocator SearchButton => testContext.Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Search" });
}