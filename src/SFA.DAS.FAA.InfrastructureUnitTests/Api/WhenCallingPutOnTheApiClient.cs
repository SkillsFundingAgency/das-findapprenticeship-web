using FluentAssertions;
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
using System.Net;

namespace SFA.DAS.FAA.Infrastructure.UnitTests.Api;
public class WhenCallingPutOnTheApiClient
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
        var putTestRequest = new PutTestRequest(request);

        var response = new HttpResponseMessage
        {
            Content = new StringContent(JsonConvert.SerializeObject(testObject)),
            StatusCode = HttpStatusCode.Accepted
        };
        var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(config.BaseUrl + putTestRequest.PutUrl), config.Key, HttpMethod.Put);
        var client = new HttpClient(httpMessageHandler.Object);
        var apiClient = new ApiClient(client, configMock.Object);

        var actual = await apiClient.Put<List<string>>(putTestRequest);

        actual.Should().BeEquivalentTo(testObject);
    }

    [Test, MoqAutoData]
    public async Task Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown(
        PutCandidateApiRequest request,
        FindAnApprenticeshipOuterApi config)
    {
        config.BaseUrl = $"https://{config.BaseUrl}";
        var configMock = new Mock<IOptions<FindAnApprenticeshipOuterApi>>();
        configMock.Setup(x => x.Value).Returns(config);
        var putTestRequest = new PutTestRequest(request);
        var response = new HttpResponseMessage
        {
            Content = new StringContent(""),
            StatusCode = HttpStatusCode.BadRequest
        };

        var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(config.BaseUrl + putTestRequest.PutUrl), config.Key, HttpMethod.Put);
        var client = new HttpClient(httpMessageHandler.Object);
        var apiClient = new ApiClient(client, configMock.Object);

        Func<Task> actual = () => apiClient.Put<List<string>>(putTestRequest);

        await actual.Should().ThrowExactlyAsync<HttpRequestException>();
    }

    [Test, MoqAutoData]
    public async Task Then_If_It_Is_Not_Found_Default_Is_Returned(
        PutCandidateApiRequest request,
        FindAnApprenticeshipOuterApi config)
    {
        config.BaseUrl = $"https://{config.BaseUrl}";
        var configMock = new Mock<IOptions<FindAnApprenticeshipOuterApi>>();
        configMock.Setup(x => x.Value).Returns(config);
        var putTestRequest = new PutTestRequest(request);
        var response = new HttpResponseMessage
        {
            Content = new StringContent(""),
            StatusCode = HttpStatusCode.NotFound
        };

        var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(config.BaseUrl + putTestRequest.PutUrl), config.Key, HttpMethod.Put);
        var client = new HttpClient(httpMessageHandler.Object);
        var apiClient = new ApiClient(client, configMock.Object);

        var actual = await apiClient.Put<List<string>>(putTestRequest);

        actual.Should().BeNull();
    }

    private class PutTestRequest : IPutApiRequest
    {
        public PutTestRequest(PutCandidateApiRequest request)
        {
            Data = request;
        }
        public string PutUrl => $"/test-url/put";
        public object Data { get; set; }
    }
}
