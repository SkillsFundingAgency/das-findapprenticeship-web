using System.Net;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Candidates;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Infrastructure.Api;
using SFA.DAS.FAA.Infrastructure.UnitTests.HttpMessageHandlerMock;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Infrastructure.UnitTests.Api;
public class WhenCallingPutWithResponseCodeOnTheApiClient
{
    [Test, MoqAutoData]
    public async Task Then_The_Endpoint_Is_Called_With_Authentication_Header_And_Data_Returned(
        PutCandidateApiRequest request,
        List<string> testObject,
        FindAnApprenticeshipOuterApi config)
    {
        config.BaseUrl = $"https://{config.BaseUrl}";
        var configMock = new Mock<IOptions<FindAnApprenticeshipOuterApi>>();
        configMock.Setup(x => x.Value).Returns(config);
        var putTestRequest = new PutWithResponseCodeTestRequest(request);

        var response = new HttpResponseMessage
        {
            Content = new StringContent(JsonConvert.SerializeObject(testObject)),
            StatusCode = HttpStatusCode.Accepted
        };
        var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(config.BaseUrl + putTestRequest.PutUrl), config.Key, HttpMethod.Put);
        var client = new HttpClient(httpMessageHandler.Object);
        var apiClient = new ApiClient(client, configMock.Object);

        var actual = await apiClient.PutWithResponseCode<List<string>>(putTestRequest);

        using (new AssertionScope())
        {
            actual.StatusCode.Should().Be(HttpStatusCode.Accepted);
            actual.Body.Should().NotBeNullOrEmpty();
        }
    }

    [Test, MoqAutoData]
    public async Task Then_If_It_Is_Not_Found_Status_Code_Is_Returned(
        PutCandidateApiRequest request,
        FindAnApprenticeshipOuterApi config)
    {
        config.BaseUrl = $"https://{config.BaseUrl}";
        var configMock = new Mock<IOptions<FindAnApprenticeshipOuterApi>>();
        configMock.Setup(x => x.Value).Returns(config);
        var putTestRequest = new PutWithResponseCodeTestRequest(request);
        var response = new HttpResponseMessage
        {
            Content = new StringContent(""),
            StatusCode = HttpStatusCode.NotFound
        };

        var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(config.BaseUrl + putTestRequest.PutUrl), config.Key, HttpMethod.Put);
        var client = new HttpClient(httpMessageHandler.Object);
        var apiClient = new ApiClient(client, configMock.Object);

        var actual = await apiClient.PutWithResponseCode<List<string>>(putTestRequest);

        using (new AssertionScope())
        {
            actual.ErrorContent.Should().BeNull();
            actual.Body.Should().BeNull();
            actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

    private class PutWithResponseCodeTestRequest : IPutApiRequest
    {
        public PutWithResponseCodeTestRequest(PutCandidateApiRequest request)
        {
            Data = request;
        }
        public string PutUrl => $"/test-url/put";
        public object Data { get; set; }
    }
}
