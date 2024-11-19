using System.Text.RegularExpressions;
using System.Web;
using NUnit.Framework;
using SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

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
            var client = context.Get<TestHttpClient>(ContextKeys.TestHttpClient);
            var formData = new MultipartFormDataContent()
            {
                { new StringContent(value), "Data" },
            };
            
            var response = await client.PostAsync("apprenticeships/save-search", formData);
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