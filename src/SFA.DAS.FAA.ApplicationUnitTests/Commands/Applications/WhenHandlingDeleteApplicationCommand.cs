using System.Net;
using SFA.DAS.FAA.Application.Commands.Applications.Delete;
using SFA.DAS.FAA.Domain;
using SFA.DAS.FAA.Domain.Applications.DeleteApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.Applications;

public class WhenHandlingDeleteApplicationCommand
{
    [Test]
    [MoqInlineAutoData(HttpStatusCode.BadRequest)]
    [MoqInlineAutoData(HttpStatusCode.BadGateway)]
    [MoqInlineAutoData(HttpStatusCode.NotFound)]
    [MoqInlineAutoData(HttpStatusCode.NotAcceptable)]
    public async Task Handles_Invalid_Response_Code(
        HttpStatusCode statusCode,
        DeleteApplicationCommand request,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] DeleteApplicationCommandHandler sut)
    {
        // arrange
        var apiResponse = new ApiResponse<PostDeleteApplicationApiResponse>(null, statusCode, string.Empty);
        apiClient
            .Setup(x => x.PostWithResponseCodeAsync<PostDeleteApplicationApiResponse>(It.IsAny<PostDeleteApplicationApiRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        result.Success.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public async Task Handles_Success_Response_Code(
        DeleteApplicationCommand request,
        PostDeleteApplicationApiResponse response,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] DeleteApplicationCommandHandler sut)
    {
        // arrange
        var apiResponse = new ApiResponse<PostDeleteApplicationApiResponse>(response, HttpStatusCode.OK, null!);
        apiClient
            .Setup(x => x.PostWithResponseCodeAsync<PostDeleteApplicationApiResponse>(It.IsAny<PostDeleteApplicationApiRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);
        
        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        result.Should().BeEquivalentTo(response, opt => opt.ExcludingMissingMembers());
    }
}