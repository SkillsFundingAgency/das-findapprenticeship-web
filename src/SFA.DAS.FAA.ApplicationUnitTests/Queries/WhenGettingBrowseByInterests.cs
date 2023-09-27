using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries;

public class WhenGettingBrowseByInterests
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned()
    {
        // Arrange
        var apiClientMock = new Mock<IApiClient>();
        var query = new GetBrowseByInterestsQuery(); // You need to provide any required input here
        var handler = new GetBrowseByInterestsQueryHandler(apiClientMock.Object);

        var apiResponse = new BrowseByInterestsApiResponse
        {
            Routes = new List<Route>()
        };
        apiClientMock.Setup(client => client.Get<BrowseByInterestsApiResponse>(It.IsAny<GetBrowseByInterestsApiRequest>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);

    }
}