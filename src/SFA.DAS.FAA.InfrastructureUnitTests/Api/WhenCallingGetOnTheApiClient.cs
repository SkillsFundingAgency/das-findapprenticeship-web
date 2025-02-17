using FluentAssertions;
using System.Net;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Infrastructure.Api;
using SFA.DAS.FAA.Infrastructure.UnitTests.HttpMessageHandlerMock;

namespace SFA.DAS.FAA.Infrastructure.UnitTests.Api
{
    public class WhenCallingGetOnTheApiClient
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called_With_Authentication_Header_And_Data_Returned(
            List<string> testObject, 
            FindAnApprenticeshipOuterApi config)
        {
            //Arrange
            var baseUrl = $"https://{config.BaseUrl}";
            var configMock = new Mock<IOptions<FindAnApprenticeshipOuterApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var getTestRequest = new GetTestRequest();
            
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(testObject)),
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(baseUrl + getTestRequest.GetUrl), config.Key, HttpMethod.Get);
            var client = new HttpClient(httpMessageHandler.Object);
            client.BaseAddress = new Uri(baseUrl);
            var apiClient = new ApiClient(client, configMock.Object);

            //Act
            var actual = await apiClient.Get<List<string>>(getTestRequest);
            
            //Assert
            actual.Should().BeEquivalentTo(testObject);
        }
        
        [Test, AutoData]
        public void Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown(
            FindAnApprenticeshipOuterApi config)
        {
            //Arrange
            var baseUrl = $"https://{config.BaseUrl}";
            var configMock = new Mock<IOptions<FindAnApprenticeshipOuterApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var getTestRequest = new GetTestRequest();
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.BadRequest
            };
            
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(baseUrl + getTestRequest.GetUrl), config.Key, HttpMethod.Get);
            var client = new HttpClient(httpMessageHandler.Object);
            client.BaseAddress = new Uri(baseUrl);
            var apiClient = new ApiClient(client, configMock.Object);
            
            //Act Assert
            Assert.ThrowsAsync<HttpRequestException>(() => apiClient.Get<List<string>>(getTestRequest));
            
        }
        
        [Test, AutoData]
        public async Task Then_If_It_Is_Not_Found_Default_Is_Returned(
            FindAnApprenticeshipOuterApi config)
        {
            //Arrange
            var baseUrl = $"https://{config.BaseUrl}";
            var configMock = new Mock<IOptions<FindAnApprenticeshipOuterApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var getTestRequest = new GetTestRequest();
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.NotFound
            };
            
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(baseUrl + getTestRequest.GetUrl), config.Key, HttpMethod.Get);
            var client = new HttpClient(httpMessageHandler.Object);
            client.BaseAddress = new Uri(baseUrl);
            var apiClient = new ApiClient(client, configMock.Object);
            
            //Act Assert
            var actual = await apiClient.Get<List<string>>(getTestRequest);

            actual.Should().BeNull();

        }

        private class GetTestRequest : IGetApiRequest
        {
            public string GetUrl => "/test-url/get";
        }
        
        
    }
}