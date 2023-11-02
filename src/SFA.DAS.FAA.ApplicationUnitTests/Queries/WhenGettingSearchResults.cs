using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries;

public class WhenGettingSearchResults
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned()
    {
        // Arrange
        var apiClientMock = new Mock<IApiClient>();

        var handler = new GetSearchResultsQueryHandler(apiClientMock.Object);

        // Mock the response from the API client
        var expectedResponse = new GetSearchResultsApiResponse();
        apiClientMock.Setup(client => client.Get<GetSearchResultsApiResponse>(It.IsAny<GetSearchResultsApiRequest>()))
            .ReturnsAsync(expectedResponse);

        var query = new GetSearchResultsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResponse.Total, result.Total);
    }
}