using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Infrastructure.Api;
using SFA.DAS.FAA.Infrastructure.UnitTests.HttpMessageHandlerMock;
using SFA.DAS.Testing.AutoFixture;
using System.Net;


namespace SFA.DAS.FAA.Infrastructure.UnitTests.Api
{
    public class WhenCallingDeleteOnTheApiClient
    {
        [Test, MoqAutoData]
        public async Task Then_The_Endpoint_Is_Called_With_Authentication_Header(
              FindAnApprenticeshipOuterApi config)
        {
            // Arrange
            var deleteTestRequest = new DeleteTestRequest();
            config.BaseUrl = $"https://{config.BaseUrl}";
            var configMock = new Mock<IOptions<FindAnApprenticeshipOuterApi>>();
            configMock.Setup(x => x.Value).Returns(config);

            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                })
                .Verifiable();

            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);

            // Act
            await apiClient.Delete(deleteTestRequest);

            // Assert
            httpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Delete &&
                    req.RequestUri == new Uri(config.BaseUrl + deleteTestRequest.DeleteUrl) &&
                    req.Headers.Contains("Authorization") &&
                    req.Headers.GetValues("Authorization").Contains(config.Key)
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test, MoqAutoData]
        public async Task Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown(
            FindAnApprenticeshipOuterApi config)
        {
            var deleteTestRequest = new DeleteTestRequest();
            config.BaseUrl = $"https://{config.BaseUrl}";
            var configMock = new Mock<IOptions<FindAnApprenticeshipOuterApi>>();
            configMock.Setup(x => x.Value).Returns(config);

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(config.BaseUrl + deleteTestRequest.DeleteUrl), config.Key, HttpMethod.Delete);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);

            // Act
            Func<Task> actual = async () => await apiClient.Delete(deleteTestRequest);

            // Assert
            await actual.Should().ThrowAsync<HttpRequestException>();
        }

        private class DeleteTestRequest : IDeleteApiRequest
        {
            public string DeleteUrl => "/test-url/delete";
        }
    }
}

