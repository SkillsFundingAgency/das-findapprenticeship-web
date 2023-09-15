using System.Net;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Infrastructure.Api;
using SFA.DAS.FAA.Infrastructure.UnitTests.HttpMessageHandlerMock;

namespace SFA.DAS.FAA.Infrastructure.UnitTests.Api
{
    public class WhenCallingPostOnTheApiClient
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called(
            Guid responseId,
            PostData postContent,
            int id,
            FindAnApprenticeshipApi config)
        {
            //Arrange
            var configMock = new Mock<IOptions<FindAnApprenticeshipApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var response = new HttpResponseMessage
            {
                Content = new StringContent($"'{responseId}'"),
                StatusCode = HttpStatusCode.Accepted
            };
            var postTestRequest = new PostTestRequest(id,"https://test.local") {Data = postContent};
            var expectedUrl = postTestRequest.PostUrl;
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, expectedUrl, config.Key, HttpMethod.Post);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);
            
            
            //Act
            var actual = await apiClient.Post<string,PostData>(postTestRequest);

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Post)
                        && c.RequestUri.AbsoluteUri.Equals(expectedUrl)),
                    ItExpr.IsAny<CancellationToken>()
                );
            Guid.Parse(actual).Should().Be(responseId);
        }
        
        [Test, AutoData]
        public void Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown(
            PostData postContent,
            int id,
            FindAnApprenticeshipApi config)
        {
            //Arrange
            var configMock = new Mock<IOptions<FindAnApprenticeshipApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            var postTestRequest = new PostTestRequest(id,"https://test.local") {Data = postContent};
            var expectedUrl = postTestRequest.PostUrl;
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, expectedUrl, config.Key, HttpMethod.Post);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);
            
            //Act Assert
            Assert.ThrowsAsync<HttpRequestException>(() => apiClient.Post<Guid, PostData>(postTestRequest));
            
        }
        
        private class PostTestRequest : IPostApiRequest<PostData>
        {
            private readonly int _id;

            public PostTestRequest (int id, string baseUrl)
            {
                _id = id;
                BaseUrl = baseUrl;
            }
            public PostData Data { get; set; }
            public string PostUrl => $"{BaseUrl}/test-url/post/{_id}";
            public string BaseUrl { get; }
        }

        public class PostData
        {
            public string Id { get; set; }
        }

    }
}