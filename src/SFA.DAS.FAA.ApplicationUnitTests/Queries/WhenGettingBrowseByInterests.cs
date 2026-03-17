using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Queries;

public class WhenGettingBrowseByInterests
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetBrowseByInterestsQuery query,
        BrowseByInterestsApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetBrowseByInterestsQueryHandler handler
        )
    {
        // Arrange
        apiClientMock.Setup(client => client.Get<BrowseByInterestsApiResponse>(It.IsAny<GetBrowseByInterestsApiRequest>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        apiResponse.Routes.Should().BeEquivalentTo(result.Routes);

    }
}