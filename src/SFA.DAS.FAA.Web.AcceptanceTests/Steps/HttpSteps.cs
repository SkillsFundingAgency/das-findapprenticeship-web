using System.Net;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.AcceptanceTests.Extensions;
using SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Steps;

[Binding]
public sealed class HttpSteps
{
    private readonly ScenarioContext _context;

    public HttpSteps(ScenarioContext context)
    {
        _context = context;
    }

    [When("I navigate to the following url: (.*)")]
    public async Task WhenINavigateToTheFollowingUrl(string url)
    {
        var client = _context.Get<TestHttpClient>(ContextKeys.TestHttpClient);
        var response = await client.GetAsync(url);
        var responseContent = await response.Content.ReadAsStringAsync();
        ClearResponseContext();
        _context.Set(response, ContextKeys.HttpResponse);
        _context.Set(responseContent, ContextKeys.HttpResponseContent);
    }


    [When("I navigate to the (.*) page")]
    public async Task WhenINavigateToAPage(string pageName)
    {
        var page = _context.GetPage(pageName);

        var client = _context.Get<TestHttpClient>(ContextKeys.TestHttpClient);
        var response = await client.GetAsync(page.Url);
        var responseContent = await response.Content.ReadAsStringAsync();
        ClearResponseContext();
        _context.Set(response, ContextKeys.HttpResponse);
        _context.Set(responseContent, ContextKeys.HttpResponseContent);
    }

    [Given("I post to the (.*) page")]
    [When("I post to the (.*) page")]
    public async Task WhenIPostToAPage(string pageName, Table formBody)
    {
        var page = _context.GetPage(pageName);

        var client = _context.Get<TestHttpClient>(ContextKeys.TestHttpClient);

        var contentDictionary = formBody.Rows.ToDictionary(r => r[0], r => r[1]);

        var content = new FormUrlEncodedContent(contentDictionary);

        var response = await client.PostAsync(page.Url, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        ClearResponseContext();
        _context.Set(response, ContextKeys.HttpResponse);
        _context.Set(responseContent, ContextKeys.HttpResponseContent);
    }

    [Given("I post an empty form to the (.*) page")]
    [When("I post an empty form to the (.*) page")]
    public async Task WhenIPostAnEmptyFormToAPage(string pageName)
    {
        var page = _context.GetPage(pageName);

        var client = _context.Get<TestHttpClient>(ContextKeys.TestHttpClient);

        var response = await client.PostAsync(page.Url, new StringContent(string.Empty));
        var responseContent = await response.Content.ReadAsStringAsync();
        ClearResponseContext();
        _context.Set(response, ContextKeys.HttpResponse);
        _context.Set(responseContent, ContextKeys.HttpResponseContent);
    }

    [Given("I post to the following url: (.*)")]
    [When("I post to the following url: (.*)")]
    public async Task WhenIPostToTheFollowingUrl(string url, Table formBody)
    {
        var client = _context.Get<TestHttpClient>(ContextKeys.TestHttpClient);

        var contentDictionary = formBody.Rows.ToDictionary(r => r[0], r => r[1]);

        var content = new FormUrlEncodedContent(contentDictionary);

        var response = await client.PostAsync(url, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        ClearResponseContext();
        _context.Set(response, ContextKeys.HttpResponse);
        _context.Set(responseContent, ContextKeys.HttpResponseContent);
    }

    [Given("I post an empty form to the following url: (.*)")]
    [When("I post an empty form to the following url: (.*)")]
    public async Task WhenIPostAnEmptyFormToTheFollowingUrl(string url)
    {
        var client = _context.Get<TestHttpClient>(ContextKeys.TestHttpClient);

        var response = await client.PostAsync(url, new StringContent(string.Empty));
        var responseContent = await response.Content.ReadAsStringAsync();
        ClearResponseContext();
        _context.Set(response, ContextKeys.HttpResponse);
        _context.Set(responseContent, ContextKeys.HttpResponseContent);
    }

    [Then("a http status code of (.*) is returned")]
    public void ThenAHttpStatusCodeIsReturned(int httpStatusCode)
    {
        if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
        {
            Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
        }

        result.StatusCode.Should().Be((HttpStatusCode)httpStatusCode);
    }

    [Then("I am redirected to the following url: (.*)")]
    public async Task ThenIAmRedirectedToTheFollowingUrl(string url)
    {
        var redirection = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);
        redirection.StatusCode.Should().Be(HttpStatusCode.Redirect);
        redirection.Headers.Location.ToString().Should().StartWith(url);

        var client = _context.Get<TestHttpClient>(ContextKeys.TestHttpClient);
        var response = await client.GetAsync(url);
        var responseContent = await response.Content.ReadAsStringAsync();
        if (_context.ContainsKey(ContextKeys.HttpResponseRedirectContent))
        {
            _context.Remove(ContextKeys.HttpResponseRedirectContent);
        }

        _context.Set(responseContent, ContextKeys.HttpResponseRedirectContent);
    }

    [Then("I am redirected to the (.*) page")]
    public async Task ThenIAmRedirectedToAPage(string pageName)
    {
        var page = _context.GetPage(pageName);

        var redirection = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);
        redirection.StatusCode.Should().Be(HttpStatusCode.Redirect);
        redirection.Headers.Location.ToString().Should().StartWith(page.Url);

        var client = _context.Get<TestHttpClient>(ContextKeys.TestHttpClient);
        var response = await client.GetAsync(page.Url);
        var responseContent = await response.Content.ReadAsStringAsync();
        if (_context.ContainsKey(ContextKeys.HttpResponseRedirectContent))
        {
            _context.Remove(ContextKeys.HttpResponseRedirectContent);
        }

        _context.Set(responseContent, ContextKeys.HttpResponseRedirectContent);
    }

    private void ClearResponseContext()
    {
        if (_context.ContainsKey(ContextKeys.HttpResponse))
        {
            _context.Remove(ContextKeys.HttpResponse);
        }

        if (_context.ContainsKey(ContextKeys.HttpResponseContent))
        {
            _context.Remove(ContextKeys.HttpResponseContent);
        }
    }
}