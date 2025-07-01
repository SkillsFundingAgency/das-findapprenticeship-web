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

    [StepDefinition("I go to delete saved search page")]
    public async Task WhenIClickTheDeleteSavedSearch()
    {
        var content = context.Get<string>(ContextKeys.HttpResponseContent);
        var data = Regex.Match(content, "<a class=\"das-button--inline-link govuk-link--no-visited-state\" href=\"/saved-searches/(?<val>.*)\"");
    
        var savedSearchId = HttpUtility.HtmlDecode(data.Groups["val"].Value);
        
        var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);
        var response = await client.GetAsync($"/saved-searches/{savedSearchId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        context.ClearResponseContext();
        context.Set(response, ContextKeys.HttpResponse);
        context.Set(responseContent, ContextKeys.HttpResponseContent);
        context.Set(savedSearchId, ContextKeys.SavedSearchId);
    
    }
    
    [StepDefinition("I delete the saved search")]
    public async Task WhenIDeleteTheSavedSearch()
    {
        var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);
        var savedSearchId = context.Get<string>(ContextKeys.SavedSearchId);
        
        var response = await client.PostAsync($"/saved-searches/{savedSearchId}", new Dictionary<string, string>());
        var responseContent = await response.Content.ReadAsStringAsync();
        context.ClearResponseContext();
        context.Set(response, ContextKeys.HttpResponse);
        context.Set(responseContent, ContextKeys.HttpResponseContent);   
    }
}