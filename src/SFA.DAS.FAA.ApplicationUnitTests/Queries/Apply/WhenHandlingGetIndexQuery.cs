using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
using SFA.DAS.FAA.Domain.Apply.GetIndex;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

public class WhenHandlingGetIndexQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetIndexQuery query,
        GetIndexApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetIndexQueryHandler handler)
    {
        // Arrange
        var apiRequestUri = new GetIndexApiRequest(query.ApplicationId, query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetIndexApiResponse>(
                    It.Is<GetIndexApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(apiResponse);
    }
}