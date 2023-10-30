//using FluentAssertions;
//using SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;
//using TechTalk.SpecFlow;

//namespace SFA.DAS.FAA.Web.AcceptanceTests.Steps;

//[Binding]
//public sealed class ContentSteps
//{
//    private readonly ScenarioContext _context;

//    public ContentSteps(ScenarioContext context)
//    {
//        _context = context;
//    }

//    [Then("the page content includes the following: (.*)")]
//    public async Task ThenThePageContentIncludesTheFollowing(string expectedContent)
//    {
//        var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);

//        var actualContent = await response.Content.ReadAsStringAsync();

//        actualContent.Should().Contain(expectedContent);
//    }
//}
