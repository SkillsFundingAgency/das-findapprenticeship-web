using FluentAssertions;
using SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Steps;

[Binding]
public sealed class ContentSteps
{
    private readonly ScenarioContext _context;

    public ContentSteps(ScenarioContext context)
    {
        _context = context;
    }

    [Then("the page content includes the following error: (.*)")]
    public async Task ThenThePageContentIncludesTheFollowingError(string expectedContent)
    {
        var response = _context.Get<string>(ContextKeys.HttpResponseContent);

        response.Should().Contain(expectedContent);
        response.Should().Contain("There is a problem");
    }

    [Then("the page redirect content includes the following: (.*)")]
    public async Task ThenThePageRedirectContentIncludesTheFollowing(string expectedContent)
    {
        var response = _context.Get<string>(ContextKeys.HttpResponseRedirectContent);

        response.Should().Contain(expectedContent);
    }

    [Then("the page content includes the following: (.*)")]
    public async Task ThenThePageContentIncludesTheFollowing(string expectedContent)
    {
        var response = _context.Get<string>(ContextKeys.HttpResponseContent);

        response.Should().Contain(expectedContent);
    }
}
