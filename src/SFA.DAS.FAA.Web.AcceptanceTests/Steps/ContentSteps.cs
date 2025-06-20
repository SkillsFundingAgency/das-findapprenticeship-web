using System.Web;
using FluentAssertions;
using Reqnroll;
using SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;

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
        var decodedResponse = HttpUtility.HtmlDecode(response);

        decodedResponse.Should().Contain(expectedContent);
        decodedResponse.Should().Contain("There is a problem");
    }

    /// <summary>
    /// Used to check content on page. If the content is only on environment, surround with ^ ^
    /// eg. Save search ^for some search^ deleted
    /// </summary>
    /// <param name="expectedContent">The content to check on page</param>
    [Then("the page redirect content includes the following: (.*)")]
    public async Task ThenThePageRedirectContentIncludesTheFollowing(string expectedContent)
    {
        var response = _context.Get<string>(ContextKeys.HttpResponseRedirectContent);

        if (expectedContent.Contains("^") && _context.Get<string>(ContextKeys.Environment) == "")
        {
            expectedContent = expectedContent.Remove(expectedContent.IndexOf("^"),
                expectedContent.LastIndexOf("^") - expectedContent.IndexOf("^")).Replace(" ^","");
        }
        else
        {
            expectedContent = expectedContent.Replace("^", "");
        }
        
        response.Should().Contain(expectedContent);
    }

    [Then("the page content includes the following: (.*)")]
    public async Task ThenThePageContentIncludesTheFollowing(string expectedContent)
    {
        var response = _context.Get<string>(ContextKeys.HttpResponseContent);

        response.Should().Contain(expectedContent);
    }
}
