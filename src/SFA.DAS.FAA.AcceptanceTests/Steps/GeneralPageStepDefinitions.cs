using Reqnroll;
using SFA.DAS.FAA.AcceptanceTests.Core;

namespace SFA.DAS.FAA.AcceptanceTests.Steps;

[Binding]
public class GeneralPageStepDefinitions(ITestContext testContext)
{
    private ILocator CookieAcceptButton => testContext.Page.GetByRole(AriaRole.Button, new() { Name = "Accept additional cookies" });
    private ILocator CookieRejectButton => testContext.Page.GetByRole(AriaRole.Button, new() { Name = "Reject additional cookies" });
    
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
    
    [When(@"I accept the cookie preferences from the banner")]
    public async Task WhenIAcceptTheCookiePreferencesFromTheBanner()
    {
        await CookieAcceptButton.ClickAsync();
    }

    [When(@"I reject the cookie preferences from the banner")]
    public async Task WhenIRejectTheCookiePreferencesFromTheBanner()
    {
        await CookieRejectButton.ClickAsync();
    }

    [Then(@"the cookie banner confirms that I rejected cookies")]
    public async Task ThenTheCookieBannerConfirmsThatIRejectedCookies()
    {
        (await testContext.Page.GetByText("You’ve rejected analytics cookies").IsVisibleAsync()).Should().BeTrue();
    }

    [Then(@"the ""(.*)"" cookie is set to ""(.*)""")]
    public async Task ThenTheCookieIsSetTo(string cookieName, string cookieValue)
    {
        var cookies = await testContext.Page.Context.CookiesAsync();
        var cookie = cookies.SingleOrDefault(x => x.Name == cookieName);
        cookie.Should().NotBeNull();
        cookie!.Value.Should().Be(cookieValue);
    }

    [Then(@"the cookie banner confirms that I accepted cookies")]
    public async Task ThenTheCookieBannerConfirmsThatIAcceptedCookies()
    {
        (await testContext.Page.GetByText("You've accepted additional cookies").IsVisibleAsync()).Should().BeTrue();
    }

    
}