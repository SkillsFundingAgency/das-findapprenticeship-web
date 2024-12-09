using SFA.DAS.FAA.AcceptanceTests.Core;

namespace SFA.DAS.FAA.AcceptanceTests.Pages;

public class SignInPage(ITestContext testContext)
{
    public const string PageUrl = "account-details";
    
    private ILocator IdInput => testContext.Page.GetByLabel("ID");
    private ILocator EmailInput => testContext.Page.GetByLabel("Email");
    private ILocator MobilePhoneInput => testContext.Page.GetByLabel("Mobile phone");
    private ILocator SignInButton => testContext.Page.GetByRole(AriaRole.Button, new () { Name = "Sign in" });
    
    public async Task VisitAsync()
    {
        var uri = new Uri(new Uri(testContext.ApplicationSettings.BaseUrl), PageUrl);
        var response = await testContext.Page.GotoAsync(uri.AbsoluteUri);
        response!.Ok.Should().BeTrue();
    }

    public async Task SignInAsync()
    {
        await IdInput.FillAsync(testContext.ApplicationSettings.User?.Id);
        await EmailInput.FillAsync(testContext.ApplicationSettings.User?.Email);
        await MobilePhoneInput.FillAsync(testContext.ApplicationSettings.User?.MobilePhone);
        await SignInButton.ClickAsync();
        // TODO: assert successful sign in
    }
}