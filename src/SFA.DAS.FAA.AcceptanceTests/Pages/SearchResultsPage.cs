using SFA.DAS.FAA.AcceptanceTests.Core;
using SFA.DAS.FAA.AcceptanceTests.Models;

namespace SFA.DAS.FAA.AcceptanceTests.Pages;

public class SearchResultsPage(ITestContext testContext)
{
    internal const string PageUrl = "apprenticeships";
    public ILocator WhatInput => testContext.Page.GetByLabel("What");
    public ILocator WhereInput => testContext.Page.GetByLabel("Where");
    private ILocator WhereSelectListOption0 => testContext.Page.Locator("li[id='WhereSearchTerm__option--0']");
    
    private ILocator SearchButton => testContext.Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Search" });
    
    public async Task VisitAsync()
    {
        var currentUri = new Uri(testContext.Page.Url);
        if (currentUri.AbsolutePath == PageUrl)
        {
            return;
        }
        
        var uri = new Uri(new Uri(testContext.ApplicationSettings.BaseUrl), PageUrl);
        await testContext.Page.GotoAsync(uri.AbsoluteUri);
    }

    public async Task SearchAsync(SearchCriteria searchCriteria)
    {
        if (searchCriteria.What is not null)
        {
            await WhatInput.FillAsync(searchCriteria.What);
        }

        if (searchCriteria.Where is not null)
        {
            await WhereInput.FillAsync(searchCriteria.Where);
            await WhereSelectListOption0.WaitForAsync(new LocatorWaitForOptions()
            {
                Timeout = 3000
            });
            await WhereSelectListOption0.ClickAsync();
            searchCriteria.Where = await WhereInput.InputValueAsync();
        }
        
        // TODO: more search options

        await SearchButton.ClickAsync();
        testContext.SearchCriteria = searchCriteria;
    }
}