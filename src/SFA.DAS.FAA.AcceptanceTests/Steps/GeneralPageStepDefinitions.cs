using Reqnroll;

namespace SFA.DAS.FAA.AcceptanceTests.Steps;

[Binding]
public class GeneralPageStepDefinitions(ITestContext testContext)
{
    [Then(@"the page title is ""(.*)""")]
    public async Task ThenThePageTitleIs(string text)
    {
        (await testContext.Page.TitleAsync()).Should().Be(text);
    }

    [Then(@"the page heading is ""(.*)""")]
    public async Task ThenThePageHeadingIs(string text)
    {
        (await testContext.Page.Locator("h1:first-of-type").TextContentAsync()).Should().Be(text);
    }
}