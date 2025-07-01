using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using Reqnroll;
using SFA.DAS.FAA.Web.AcceptanceTests.Extensions;
using SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Steps;

[Binding]
public sealed class HttpSteps(ScenarioContext context)
{
    [When("I navigate to the following url: (.*)")]
    public async Task WhenINavigateToTheFollowingUrl(string url)
    {
        var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);
        var response = await client.GetAsync(url);
        var responseContent = await response.Content.ReadAsStringAsync();
        context.ClearResponseContext();
        context.Set(response, ContextKeys.HttpResponse);
        context.Set(responseContent, ContextKeys.HttpResponseContent);
    }

    [When("I navigate to the (.*) page")]
    public async Task WhenINavigateToAPage(string pageName)
    {
        var page = context.GetPage(pageName);

        var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);
        var response = await client.GetAsync(page.Url);
        var responseContent = await response.Content.ReadAsStringAsync();
        context.ClearResponseContext();
        context.Set(response, ContextKeys.HttpResponse);
        context.Set(responseContent, ContextKeys.HttpResponseContent);
    }

    [When("I navigate to the (.*) page with querystring parameters")]
    public async Task WhenINavigateToAPage(string pageName, Table querystring)
    {
        var page = context.GetPage(pageName);

        var url = new StringBuilder(page.Url);
        url.Append("?");
        var querystringParameters = querystring.Rows.ToDictionary(r => r[0], r => r[1]);
        foreach (var querystringParameter in querystringParameters)
        {
            url.Append(querystringParameter.Key);
            url.Append("=");
            url.Append(querystringParameter.Value);
        }

        var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);
        var response = await client.GetAsync($"{url}");
        var responseContent = await response.Content.ReadAsStringAsync();
        context.ClearResponseContext();
        context.Set(response, ContextKeys.HttpResponse);
        context.Set(responseContent, ContextKeys.HttpResponseContent);
    }

    [Given("I post to the (.*) page")]
    [When("I post to the (.*) page")]
    public async Task WhenIPostToAPage(string pageName, Table formBody)
    {
        var page = context.GetPage(pageName);

        var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);

        var contentDictionary = formBody.Rows.ToDictionary(r => r[0], r => r[1]);

        var response = await client.PostAsync(page.Url, contentDictionary);
        var responseContent = await response.Content.ReadAsStringAsync();
        context.ClearResponseContext();
        context.Set(response, ContextKeys.HttpResponse);
        context.Set(responseContent, ContextKeys.HttpResponseContent);
    }

    [Given("I post an empty form to the (.*) page")]
    [When("I post an empty form to the (.*) page")]
    public async Task WhenIPostAnEmptyFormToAPage(string pageName)
    {
        var page = context.GetPage(pageName);

        var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);

        var response = await client.PostAsync(page.Url, new Dictionary<string, string>());
        var responseContent = await response.Content.ReadAsStringAsync();
        context.ClearResponseContext();
        context.Set(response, ContextKeys.HttpResponse);
        context.Set(responseContent, ContextKeys.HttpResponseContent);
    }

    [Given("I post to the following url: (.*)")]
    [When("I post to the following url: (.*)")]
    public async Task WhenIPostToTheFollowingUrl(string url, Table formBody)
    {
        var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);

        var contentDictionary = formBody.Rows.ToDictionary(r => r[0], r => r[1]);

        var response = await client.PostAsync(url, contentDictionary);
        var responseContent = await response.Content.ReadAsStringAsync();
        context.ClearResponseContext();
        context.Set(response, ContextKeys.HttpResponse);
        context.Set(responseContent, ContextKeys.HttpResponseContent);
    }

    [Given("I post an empty form to the following url: (.*)")]
    [When("I post an empty form to the following url: (.*)")]
    public async Task WhenIPostAnEmptyFormToTheFollowingUrl(string url)
    {
        var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);

        var response = await client.PostAsync(url, new Dictionary<string, string>());
        var responseContent = await response.Content.ReadAsStringAsync();
        context.ClearResponseContext();
        context.Set(response, ContextKeys.HttpResponse);
        context.Set(responseContent, ContextKeys.HttpResponseContent);
    }
    
    [Then("the page is (.*)")]
    public void ThenPageIsReturned(string httpStatusCode)
    {
        var response = context.GetStatusCode(httpStatusCode);
        
        if (!context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
        {
            Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
        }

        result.StatusCode.Should().Be((HttpStatusCode)response.StatusCode);
    }

    [Then("I am redirected to the following url: (.*)")]
    public async Task ThenIAmRedirectedToTheFollowingUrl(string url)
    {
        var redirection = context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);
        redirection.StatusCode.Should().BeOneOf(HttpStatusCode.Redirect, HttpStatusCode.OK);

        if (redirection.StatusCode == HttpStatusCode.Redirect)
        {
            var redirectUrl = redirection.Headers.Location?.ToString(); 
            redirectUrl.Should().StartWith(url);

            var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);
            var response = await client.GetAsync(redirectUrl!);
            var responseContent = await response.Content.ReadAsStringAsync();

            context.Remove(ContextKeys.HttpResponseRedirectContent);
            context.Set(responseContent, ContextKeys.HttpResponseRedirectContent);
        }
        else if (redirection.StatusCode == HttpStatusCode.OK)
        {
            var redirectUrl = redirection.RequestMessage!.RequestUri?.ToString(); 
            redirectUrl.Should().Contain(url);

            var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);
            var response = await client.GetAsync(redirectUrl!);
            var responseContent = await response.Content.ReadAsStringAsync();

            context.Remove(ContextKeys.HttpResponseRedirectContent);
            context.Set(responseContent, ContextKeys.HttpResponseRedirectContent);
        }
    }

    [Then("I am redirected to the (.*) page")]
    public async Task ThenIAmRedirectedToAPage(string pageName)
    {
        var page = context.GetPage(pageName);

        var redirection = context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);
        redirection.StatusCode.Should().Be(HttpStatusCode.Redirect);
        var redirectUrl = redirection.Headers.Location?.ToString(); 
        redirectUrl.Should().StartWith(page.Url);

        var client = context.Get<ITestHttpClient>(ContextKeys.TestHttpClient);
        var response = await client.GetAsync(redirectUrl!);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        context.Remove(ContextKeys.HttpResponseRedirectContent);
        context.Set(responseContent, ContextKeys.HttpResponseRedirectContent);
    }
    
    
    [Then("I am shown the (.*) page")]
    public async Task ThenIAmShownAPage(string pageName)
    {
        var page = context.GetPage(pageName);

        var redirection = context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);
        var redirectUrl = redirection.Headers.Location?.ToString();
        var getUrl = redirection.RequestMessage!.RequestUri?.ToString(); 
        redirectUrl.Should().Contain(getUrl);    
        
    }
}