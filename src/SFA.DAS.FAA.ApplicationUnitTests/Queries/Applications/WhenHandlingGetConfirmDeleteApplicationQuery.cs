using System.Net;
using SFA.DAS.FAA.Application.Queries.Applications.Delete;
using SFA.DAS.FAA.Domain;
using SFA.DAS.FAA.Domain.Applications.DeleteApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Applications;

public class WhenHandlingGetConfirmDeleteApplicationQuery
{
    [Test]
    [MoqInlineAutoData(HttpStatusCode.BadRequest)]
    [MoqInlineAutoData(HttpStatusCode.BadGateway)]
    [MoqInlineAutoData(HttpStatusCode.NotFound)]
    [MoqInlineAutoData(HttpStatusCode.NotAcceptable)]
    public async Task Handles_Invalid_Response_Code(
        HttpStatusCode statusCode,
        GetDeleteApplicationQuery request,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] GetDeleteApplicationQueryHandler sut)
    {
        // arrange
        var apiResponse = new ApiResponse<GetConfirmDeleteApplicationApiResponse>(null, statusCode, string.Empty);
        apiClient
            .Setup(x => x.GetWithResponseCodeAsync<GetConfirmDeleteApplicationApiResponse>(It.IsAny<GetConfirmDeleteApplicationApiRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        result.Should().Be(GetDeleteApplicationQueryResult.None);
    }
    
    [Test, MoqAutoData]
    public async Task Handles_Success_Response_Code(
        HttpStatusCode statusCode,
        GetConfirmDeleteApplicationApiResponse response,
        GetDeleteApplicationQuery request,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] GetDeleteApplicationQueryHandler sut)
    {
        // arrange
        var apiResponse = new ApiResponse<GetConfirmDeleteApplicationApiResponse>(response, HttpStatusCode.OK, string.Empty);
        apiClient
            .Setup(x => x.GetWithResponseCodeAsync<GetConfirmDeleteApplicationApiResponse>(It.IsAny<GetConfirmDeleteApplicationApiRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        result.Should().BeEquivalentTo(response, opt => opt.ExcludingMissingMembers());
        result.Addresses.Should().Contain(response.Address!);
        result.Addresses.Should().Contain(response.OtherAddresses);
    }
}