using System.Net;
using System.Security.Policy;
using FluentAssertions;
using NUnit.Framework;
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
        redirection.Headers.Location.Should().Be(url);

        var client = _context.Get<TestHttpClient>(ContextKeys.TestHttpClient);
        var response = await client.GetAsync(url);
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