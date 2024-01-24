using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Infrastructure.Api;
using SFA.DAS.FAA.Infrastructure.UnitTests.HttpMessageHandlerMock;
using System.Net;

namespace SFA.DAS.FAA.Infrastructure.UnitTests.Api
{
    public class WhenCallingPatchOnTheApiClient
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called_With_Authentication_Header_And_Data_Returned(
            List<string> testObject,
            FindAnApprenticeshipOuterApi config)
        {
            //Arrange
            config.BaseUrl = $"https://{config.BaseUrl}";
            var configMock = new Mock<IOptions<FindAnApprenticeshipOuterApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var patchTestRequest = new PatchTestRequest();

            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(testObject)),
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(config.BaseUrl + patchTestRequest.PatchUrl), config.Key, HttpMethod.Patch);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);

            //Act
            var actual = await apiClient.PatchWithResponseCode<List<string>>(patchTestRequest);

            //Assert
            actual.Should().BeEquivalentTo(testObject);
        }

        [Test, AutoData]
        public async Task Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown(
            FindAnApprenticeshipOuterApi config)
        {
            //Arrange
            config.BaseUrl = $"https://{config.BaseUrl}";
            var configMock = new Mock<IOptions<FindAnApprenticeshipOuterApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var patchTestRequest = new PatchTestRequest();
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.BadRequest
            };

            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(config.BaseUrl + patchTestRequest.PatchUrl), config.Key, HttpMethod.Patch);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);

            //Act Assert
            var actual = await apiClient.PatchWithResponseCode<List<string>>(patchTestRequest);

            //Assert
            actual.Should().BeNull();
        }

        [Test, AutoData]
        public async Task Then_If_It_Is_Not_Found_Default_Is_Returned(
            FindAnApprenticeshipOuterApi config)
        {
            //Arrange
            config.BaseUrl = $"https://{config.BaseUrl}";
            var configMock = new Mock<IOptions<FindAnApprenticeshipOuterApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var patchTestRequest = new PatchTestRequest();
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.NotFound
            };

            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(config.BaseUrl + patchTestRequest.PatchUrl), config.Key, HttpMethod.Patch);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);

            //Act Assert
            var actual = await apiClient.PatchWithResponseCode<List<string>>(patchTestRequest);

            actual.Should().BeNull();
        }
    }

    public class PatchTestRequest : IPatchApiRequest
    {
        public string PatchUrl => "/test-url/patch";
        public object Data { get; set; }
    }
}
