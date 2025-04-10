using SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocations;
using SFA.DAS.FAA.Domain.Apply.GetEmploymentLocations;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

[TestFixture]
public class WhenHandlingGetEmploymentLocationsQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetEmploymentLocationsQuery query,
        GetEmploymentLocationsApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetEmploymentLocationsQueryHandler handler)
    {
        // Arrange
        var apiRequestUri = new GetEmploymentLocationsApiRequest(query.ApplicationId, query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetEmploymentLocationsApiResponse>(
                    It.Is<GetEmploymentLocationsApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(apiResponse);
    }
}