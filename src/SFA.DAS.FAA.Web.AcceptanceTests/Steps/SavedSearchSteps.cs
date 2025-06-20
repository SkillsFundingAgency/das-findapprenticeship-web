using System.Text.RegularExpressions;
using System.Web;
using NUnit.Framework;
using Reqnroll;
using SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Steps;

[Binding]
public sealed class SavedSearchSteps(ScenarioContext context)
{
    [When("I save my search")]
    public async Task WhenISaveMySearch()
    {
        var content = context.Get<string>(ContextKeys.HttpResponseContent);
        var data = Regex.Match(content, "<input name=\"Data\" type=\"hidden\" value=\"(?<val>.*)\"");
        if (data is { Captures.Count: 1 })
        {
            var value = HttpUtility.HtmlDecode(data.Groups["val"].Value);
            var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);
            var token = AcceptanceTestHttpClient.ExtractRequestVerificationToken(content);
            
            var response = await client.PostAsync("apprenticeships/save-search", new Dictionary<string, string>{{"Data", value},{"__RequestVerificationToken",token}});
            var responseContent = await response.Content.ReadAsStringAsync();
            context.ClearResponseContext();
            context.Set(response, ContextKeys.HttpResponse);
        }
        else
        {
            Assert.Fail("The response does not contain the required encoded search data");
        }
    }
    
}