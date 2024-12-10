using SFA.DAS.FAA.AcceptanceTests.Core;
using SFA.DAS.FAA.AcceptanceTests.Steps;

namespace SFA.DAS.FAA.AcceptanceTests.Pages;

public class HomePage(ITestContext testContext)
{
    private const string PageUrl = "apprenticeshipsearch";
    private ILocator WhatInput => testContext.Page.GetByLabel("What");
    private ILocator WhereInput => testContext.Page.GetByLabel("Where");
    private ILocator WhereSelectListOption0 => testContext.Page.Locator("li[id='WhereSearchTerm__option--0']");
    private ILocator SearchButton => testContext.Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Search" });
    
    public async Task VisitAsync()
    {
        var uri = new Uri(new Uri(testContext.ApplicationSettings.Value.BaseUrl), PageUrl);
        await testContext.Page.GotoAsync(uri.AbsoluteUri);
        CheckUrl();
    }

    public void CheckUrl()
    {
        var uri = new Uri(testContext.Page.Url);
        uri.AbsolutePath.Should().EndWith($"/{PageUrl}");
    }

    public async Task SearchAsync(BasicSearch basicSearch)
    {
        if (basicSearch.What is not null)
        {
            await WhatInput.FillAsync(basicSearch.What);
        }

        if (basicSearch.Where is not null)
        {
            await WhereInput.FillAsync(basicSearch.Where);
            await WhereSelectListOption0.WaitForAsync(new LocatorWaitForOptions()
            {
                Timeout = 3000
            });
            await WhereSelectListOption0.ClickAsync();
            basicSearch.Where = await WhereInput.InputValueAsync();
        }

        await SearchButton.ClickAsync();
        testContext.SearchCriteria = new SearchCriteria(basicSearch.What ?? string.Empty, basicSearch.Where ?? string.Empty);
    }
}
