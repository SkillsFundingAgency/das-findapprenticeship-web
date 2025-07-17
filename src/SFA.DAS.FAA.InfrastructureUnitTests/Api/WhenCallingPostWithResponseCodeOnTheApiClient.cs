using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Moq.Protected;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Infrastructure.UnitTests.Api;

public class WhenCallingPostWithResponseCodeOnTheApiClient
{
    [Test, MoqAutoData]
    public async Task The_Headers_Are_Set_Correctly(
        IPostApiRequest request,
        HttpResponseMessage response,
        IOptions<FindAnApprenticeshipOuterApi> config,
        Mock<HttpMessageHandler> httpMessageHandler)
    {
        // arrange
        HttpRequestMessage? capturedRequest = null;
        httpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((msg, _) => { capturedRequest = msg; })
            .ReturnsAsync(response);
        
        var httpClient = new HttpClient(httpMessageHandler.Object);
        httpClient.BaseAddress = new Uri($"https://{config.Value.BaseUrl!}");
        var sut = new ApiClient(httpClient, config);
        
        // act
        await sut.PostWithResponseCodeAsync<NullResponse>(request);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Headers.Should().Contain(x => x.Key == "Ocp-Apim-Subscription-Key" && x.Value.Contains(config.Value.Key));
        capturedRequest.Headers.Should().Contain(x => x.Key == "X-Version" && x.Value.Contains("1"));
    }
    
    [Test, MoqAutoData]
    public async Task The_Request_Url_And_Method_Are_Set_Correctly(
        IPostApiRequest request,
        HttpResponseMessage response,
        IOptions<FindAnApprenticeshipOuterApi> config,
        Mock<HttpMessageHandler> httpMessageHandler)
    {
        // arrange
        HttpRequestMessage? capturedRequest = null;
        httpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((msg, _) => { capturedRequest = msg; })
            .ReturnsAsync(response);
        
        var httpClient = new HttpClient(httpMessageHandler.Object);
        httpClient.BaseAddress = new Uri($"https://{config.Value.BaseUrl!}");
        var sut = new ApiClient(httpClient, config);
        
        // act
        await sut.PostWithResponseCodeAsync<List<string>>(request);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Post);
        capturedRequest.RequestUri.Should().Be(new Uri($"https://{config.Value.BaseUrl!}/{request.PostUrl}"));
    }
    
    [Test, MoqAutoData]
    public async Task The_Request_Json_Is_Set_Correctly(
        List<string> data,
        IPostApiRequest request,
        HttpResponseMessage response,
        IOptions<FindAnApprenticeshipOuterApi> config,
        Mock<HttpMessageHandler> httpMessageHandler)
    {
        // arrange
        request.Data = data; 
        HttpRequestMessage? capturedRequest = null;
        httpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((msg, _) => { capturedRequest = msg; })
            .ReturnsAsync(response);
        
        var httpClient = new HttpClient(httpMessageHandler.Object);
        httpClient.BaseAddress = new Uri($"https://{config.Value.BaseUrl!}");
        var sut = new ApiClient(httpClient, config);
        
        // act
        await sut.PostWithResponseCodeAsync<List<string>>(request);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Content.Should().BeOfType<StringContent>();
        var content = JsonSerializer.Deserialize<List<string>>(await capturedRequest.Content.ReadAsStringAsync());
        content.Should().BeEquivalentTo(data);
    }
    
    [Test, MoqAutoData]
    public async Task The_Response_Is_Returned_Correctly(
        List<string> strings,
        IPostApiRequest request,
        IOptions<FindAnApprenticeshipOuterApi> config,
        Mock<HttpMessageHandler> httpMessageHandler)
    {
        // arrange
        var response = new HttpResponseMessage
        {
            Content = new StringContent(JsonSerializer.Serialize(strings)),
            StatusCode = HttpStatusCode.OK
        };
        
        httpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
        
        var httpClient = new HttpClient(httpMessageHandler.Object);
        httpClient.BaseAddress = new Uri($"https://{config.Value.BaseUrl!}");
        var sut = new ApiClient(httpClient, config);
        
        // act
        var result = await sut.PostWithResponseCodeAsync<List<string>>(request);

        // assert
        result.Body.Should().BeEquivalentTo(strings);
        result.ErrorContent.Should().BeNull();
    }
    
    [Test, MoqAutoData]
    public async Task The_ErrorContent_Is_Returned_Correctly_When_Errored(
        IPostApiRequest request,
        IOptions<FindAnApprenticeshipOuterApi> config,
        Mock<HttpMessageHandler> httpMessageHandler)
    {
        // arrange
        var response = new HttpResponseMessage
        {
            Content = new StringContent("errorContent"),
            StatusCode = HttpStatusCode.BadRequest
        };
        
        httpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
        
        var httpClient = new HttpClient(httpMessageHandler.Object);
        httpClient.BaseAddress = new Uri($"https://{config.Value.BaseUrl!}");
        var sut = new ApiClient(httpClient, config);
        
        // act
        var result = await sut.PostWithResponseCodeAsync<List<string>>(request);

        // assert
        result.ErrorContent.Should().Be("errorContent");
        result.Body.Should().BeNull();
    }
}